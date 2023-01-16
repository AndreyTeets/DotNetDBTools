using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Extensions.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLSequencesEditor
{
    protected readonly IQueryExecutor QueryExecutor;

    public PostgreSQLSequencesEditor(IQueryExecutor queryExecutor)
    {
        QueryExecutor = queryExecutor;
    }

    public void DropOwnerAndRename_SequencesToDrop_ToTemp(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLSequence sequence in dbDiff.SequencesToDrop)
            DropOwnerAndRenameSequenceToTemp(sequence);
    }

    public void CreateSequencesWithoutOwners(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLSequence sequence in dbDiff.SequencesToCreate)
            CreateSequenceWithoutOwner(sequence);
    }

    public void Drop_RenamedToTemp_SequencesToDrop(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLSequence sequence in dbDiff.SequencesToDrop)
            Drop_RenamedToTemp_Sequence(sequence);
    }

    public void AlterSequencesExceptOwners(PostgreSQLDatabaseDiff dbDiff)
    {
        foreach (PostgreSQLSequenceDiff sequenceDiff in dbDiff.SequencesToAlter)
            AlterSequenceExceptOwner(sequenceDiff);
    }

    public void SetSequencesOwners(PostgreSQLDatabaseDiff dbDiff)
    {
        IEnumerable<PostgreSQLSequenceDiff> sequencesToSetOwners = dbDiff
            .SequencesToCreate.Where(x => x.OwnedBy != (null, null)).Select(x =>
            {
                PostgreSQLSequenceDiff sequenceDiff = x.CreateEmptySequenceDiff();
                sequenceDiff.OwnedByToSet = x.OwnedBy;
                return sequenceDiff;
            })
            .Concat(dbDiff.SequencesToAlter.Where(x => x.OwnedByToSet != (null, null) || x.OwnedByToDrop != (null, null)).Select(x =>
                new PostgreSQLSequenceDiff()
                {
                    SequenceID = x.SequenceID,
                    NewSequenceName = x.NewSequenceName,
                    OldSequenceName = x.NewSequenceName,
                    OwnedByToSet = x.OwnedByToSet,
                    OwnedByToDrop = x.OwnedByToDrop,
                }));

        foreach (PostgreSQLSequenceDiff sequenceDiff in sequencesToSetOwners)
            AlterSequence(sequenceDiff);
    }

    private void DropOwnerAndRenameSequenceToTemp(PostgreSQLSequence sequence)
    {
        PostgreSQLSequenceDiff sequenceDiff = sequence.CreateEmptySequenceDiff();
        sequenceDiff.OwnedByToDrop = sequence.OwnedBy;
        sequenceDiff.NewSequenceName = $"_DNDBTTemp_{sequence.Name}";
        QueryExecutor.Execute(new PostgreSQLAlterSequenceQuery(sequenceDiff));
        QueryExecutor.Execute(new PostgreSQLDeleteDNDBTDbObjectRecordQuery(sequence.ID));
    }

    private void CreateSequenceWithoutOwner(PostgreSQLSequence sequence)
    {
        PostgreSQLSequence sequenceWithoutOwner = sequence.CopyModel();
        sequenceWithoutOwner.OwnedBy = (null, null);
        QueryExecutor.Execute(new PostgreSQLCreateSequenceQuery(sequenceWithoutOwner));
        QueryExecutor.Execute(new PostgreSQLInsertDNDBTDbObjectRecordQuery(sequence.ID, null, DbObjectType.Sequence, sequence.Name));
    }

    private void Drop_RenamedToTemp_Sequence(PostgreSQLSequence sequence)
    {
        PostgreSQLSequence renamedSequence = sequence.CopyModel();
        renamedSequence.Name = $"_DNDBTTemp_{sequence.Name}";
        QueryExecutor.Execute(new PostgreSQLDropSequenceQuery(renamedSequence));
    }

    private void AlterSequenceExceptOwner(PostgreSQLSequenceDiff sequenceDiff)
    {
        PostgreSQLSequenceDiff sequenceDiffExceptOwner = sequenceDiff.CopyModel();
        sequenceDiffExceptOwner.OwnedByToSet = (null, null);
        sequenceDiffExceptOwner.OwnedByToDrop = (null, null);
        if (!AnalysisManager.DiffIsEmpty(sequenceDiffExceptOwner))
            AlterSequence(sequenceDiffExceptOwner);
    }

    private void AlterSequence(PostgreSQLSequenceDiff sequenceDiff)
    {
        QueryExecutor.Execute(new PostgreSQLAlterSequenceQuery(sequenceDiff));
        QueryExecutor.Execute(new PostgreSQLUpdateDNDBTDbObjectRecordQuery(sequenceDiff.SequenceID, sequenceDiff.NewSequenceName));
    }
}
