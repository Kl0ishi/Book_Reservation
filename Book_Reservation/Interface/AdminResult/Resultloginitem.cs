namespace Book_Reservation.Interface.AdminResult
{
    public class Resultloginitem
    {
        public bool result { get; set; }
        public ResultDetails resultDetails { get; set; }
        public object resultErrs { get; set; }
    }
    public class  ResultDetails
    {
        public string personId { get; set; }
        public string fullName_TH { get; set; }
        public string fullName_EN { get; set; }
        public string position { get; set; }
        public string role { get; set; }
        public string departmentSid { get; set; }
        public string departmentSub { get; set; }
        public string departmentMid { get; set; }
        public string departmentMain { get; set; }
    }
}
