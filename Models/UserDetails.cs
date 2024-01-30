namespace RFCWEBAPIMVC.Models
{
    public class UserDetails
    {
        public int emp_epf { get; set; }
        public string emp_name { get; set; }
        public int emp_id { get; set; }
        public int company_id { get; set; }
        public int company_code { get; set; }
        public string company_name { get; set; }
        public int emp_user_grp { get; set; }
        public int emp_user_sub_grp { get; set; }
        public string emp_email { get; set; }
        public int emp_function { get; set; }
        public int emp_location { get; set; }
        public int emp_plant { get; set; }
        public int emp_mobile { get; set; }
        public int func_id { get; set; }
        public string func_name { get; set; }
        public int cost_center { get; set; }
        public int cost_id { get; set; }
        public string cost_code { get; set; }
        public string cost_name { get; set; }
        public int loc_id { get; set; }
        public string loc_name { get; set; }
        public string plant_name { get; set; }
        public int plant_code { get; set; }
        public int plant_id { get; set; }
        public byte is_active { get; set; }

    }
}