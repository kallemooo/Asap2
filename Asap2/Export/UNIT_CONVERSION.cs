namespace Asap2
{
    /// <summary>
    /// Specifies slope and offset of a linear formula to convert the referenced unit (REF_UNIT) to this UNIT.
    /// </summary>
    [Base(IsSimple = true)]
    public class UNIT_CONVERSION : Asap2Base
    {
        public UNIT_CONVERSION(decimal Gradient, decimal Offset)
        {
            this.Gradient = Gradient;
            this.Offset = Offset;
        }
        [Element(0, IsArgument = true, Comment = " Gradient ")]
        public decimal Gradient;
        [Element(1, IsArgument = true, Comment = " Offset   ")]
        public decimal Offset;
    }
}
