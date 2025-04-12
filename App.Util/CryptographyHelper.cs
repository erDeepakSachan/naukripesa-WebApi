namespace App.Util
{
    public class CryptographyHelper
    {
        private EncodingBaseTypes m_encoding;

        public CryptographyHelper(EncodingBaseTypes encoding_type = EncodingBaseTypes.Hex)
        {
            if (encoding_type != EncodingBaseTypes.Hex & encoding_type != EncodingBaseTypes.Base64)
                encoding_type = EncodingBaseTypes.Hex;
            this.EncodingType = encoding_type;
        }

        public EncodingBaseTypes EncodingType
        {
            get
            {
                return this.m_encoding;
            }
            set
            {
                this.m_encoding = value;
            }
        }

        public string Encrypt(string input, string password)
        {
            Cryptography cryptography = new Cryptography(password);
            var str = cryptography.Encrypt(input);
            return str;
        }

        public string Decrypt(string input, string password)
        {
            Cryptography cryptography = new Cryptography(password);
            return cryptography.Decrypt(input);
        }

        public enum EncodingBaseTypes
        {
            Hex = 1,
            Base64 = 2,
        }

        public enum HashTypes
        {
            MD5 = 1,
            SHA1 = 2,
            SHA256 = 3,
            SHA512 = 4,
        }
    }
}
