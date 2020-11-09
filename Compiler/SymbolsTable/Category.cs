namespace Compiler.SymbolsTable
{
    public enum Category
    {
        Identifier,
        Integer,
        Decimal,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        DifferentThan,
        EqualTo,
        Select,
        From,
        Where,
        Order_by,
        And,
        Or,
        Asc,
        Desc,
        EndOfFile,
        Literal,
        Field,
        Table,
        Separator
    }
}
