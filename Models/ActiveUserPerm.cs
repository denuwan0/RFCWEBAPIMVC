namespace RFCWEBAPIMVC.Models
{
    public class ActiveUserPerm
    {
        public int url_id { get; set; }
        public string menu_name { get; set; }
        public string url_value { get; set; }
        public int perm_id { get; set; }
        public int sub_grp_id { get; set; }
        public string sub_grp_name { get; set; }
        public int url_type { get; set; }
        public string url_type_name { get; set; }
        public byte is_active { get; set; }
        public byte is_view { get; set; }
        public byte is_edit { get; set; }

    }
}