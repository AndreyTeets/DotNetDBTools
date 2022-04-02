# Useful notes

## Debugging child processes
Child process debugging extensions visual studio for integration tests.

## Debugging ANTLR grammars
vscode-antlr4 extension, grammars that use actions/predicates may not work correctly in vscode for imput that depends on actions/predicates (e.g. PostgreSQLLexer.g4 uses actions+predicates for dollar-quotes handling and vscode will mostly fail to correctly parse input that contains dollar-quoted strings) https://github.com/mike-lischke/vscode-antlr4/blob/master/doc/grammar-debugging.md#actions-and-semantic-predicates
