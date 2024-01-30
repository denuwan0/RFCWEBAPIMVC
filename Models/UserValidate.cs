namespace RFCWEBAPIMVC.Models
{
    public class UserValidate
    {
        public int emp_id { get; set; }
        public int emp_epf { get; set; }
        public int emp_company { get; set; }
        public int emp_company_id { get; set; }
        public byte is_active { get; set; }
    }
}