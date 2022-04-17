using System;
using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

public static class PostgreSQLStatementsSplitter
{
    public static List<string> Split(string statementsStr)
    {
        List<string> statementsList = new();
        string unprocessedStatementsStr = statementsStr;
        int statementLength;
        while ((statementLength = FindFirstStatementLength(unprocessedStatementsStr)) > 0)
        {
            if (statementLength > 1)
            {
                string statement = unprocessedStatementsStr.Substring(0, statementLength).Trim();
                statementsList.Add(statement);
            }
            unprocessedStatementsStr = unprocessedStatementsStr.Substring(statementLength).Trim();
        }
        return statementsList;
    }

    private static int FindFirstStatementLength(string statementsStr)
    {
        if (statementsStr.Length == 0)
            return -1;

        int dollarQuoteEndCandidatePos = -1;
        string dollarQuote = "";
        char prevChar = '\0';
        State beforeCommentState = State.NotInQuotes;

        State state = State.NotInQuotes;
        int statementLength = 0;
        foreach (char c in statementsStr)
        {
            statementLength++;
            if (state != State.InsideSimpleQuotes &&
                state != State.InsideComment &&
                c == '-' && prevChar == '-')
            {
                beforeCommentState = state;
                state = State.InsideComment;
                continue;
            }

            bool wasLastStatement = statementLength == statementsStr.Length;
            switch (state)
            {
                case State.NotInQuotes:
                    if (c == ';')
                    {
                        return statementLength;
                    }
                    else if (c == '\'')
                    {
                        state = State.InsideSimpleQuotes;
                    }
                    else if (c == '$')
                    {
                        state = State.ReadingDollarQuote;
                        dollarQuote = "$";
                    }
                    break;
                case State.InsideSimpleQuotes:
                    if (c == '\'')
                        state = State.NotInQuotes;
                    break;
                case State.ReadingDollarQuote:
                    dollarQuote += c;
                    if (c == '$')
                        state = State.InsideDollarQuotes;
                    break;
                case State.InsideDollarQuotes:
                    if (c == '$')
                    {
                        if (dollarQuoteEndCandidatePos == -1)
                            dollarQuoteEndCandidatePos = 1;
                        else if (dollarQuoteEndCandidatePos == dollarQuote.Length - 1)
                            state = State.NotInQuotes;
                    }
                    else if (dollarQuoteEndCandidatePos >= 0)
                    {
                        if (dollarQuoteEndCandidatePos < dollarQuote.Length - 1 && c == dollarQuote[dollarQuoteEndCandidatePos])
                            dollarQuoteEndCandidatePos++;
                        else
                            dollarQuoteEndCandidatePos = -1;
                    }
                    break;
                case State.InsideComment:
                    if (wasLastStatement || c == '\n')
                        state = beforeCommentState;
                    break;
            }

            if (wasLastStatement && state == State.NotInQuotes)
                return statementLength;

            prevChar = c;
        }
        throw new Exception($"Failed to find first statement length in [{statementsStr}]");
    }

    private enum State
    {
        NotInQuotes,
        InsideSimpleQuotes,
        ReadingDollarQuote,
        InsideDollarQuotes,
        InsideComment,
    }
}
