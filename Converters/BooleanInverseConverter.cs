namespace F1Desktop.Converters;

public class BooleanInverseConverter : BooleanConverterBase<bool>
{
    public BooleanInverseConverter() : base(false, true)
    {
    }
}