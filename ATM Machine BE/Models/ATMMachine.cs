namespace ATM_Machine_BE.Models
{
    /// <summary>
    /// Represents an ATM machine.
    /// </summary>
    public class ATMMachine
    {
        public int Id { get; set; }
        public int Bill200 { get; set; }
        public int Bill100 { get; set; }
        public int Bill50 { get; set; }
        public int Bill20 { get; set; }
    }
}
