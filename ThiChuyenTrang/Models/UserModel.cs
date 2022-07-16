namespace ThiChuyenTrang.Models
{
    public class UserModel
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }
    public class UserInfoModel
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
    }

    public class RequestRespone
    {
        public string icon { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
}
