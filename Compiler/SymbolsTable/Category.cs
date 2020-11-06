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
        Order,
        By,
        And,
        Or,
        Asc,
        Desc,
        EndOfFile,
        Literal
    }
}
