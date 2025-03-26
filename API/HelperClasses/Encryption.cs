using System.Text;

namespace OnShopApi_s.HelperClasses
{
    public class Encryption
    {
        public string EncryptString(string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(data);
        }
    }
}
