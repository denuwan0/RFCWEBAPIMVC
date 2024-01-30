namespace RFCWEBAPIMVC.Models
{
    public class Authentication
    {
        public int emp_epf { get; set; }
        public int emp_user_grp { get; set; }
        public int emp_user_sub_grp { get; set; }
        public string emp_name { get; set; }
        public string emp_email { get; set; }
        public string emp_password { get; set; }
        public int emp_company { get; set; }
        public int emp_function { get; set; }
        public int emp_mobile { get; set; }
        public byte is_active { get; set; }
        public int emp_id { get; set; }
    }
}