// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("mSFfMU3/hWI92CONL4OtPlhoQDzeSPxlWVEt7pOJ5qGQq0fb2qdWZ+dV1vXn2tHe/VGfUSDa1tbW0tfUVdbY1+dV1t3VVdbW10oxgba6KoBSnV796P/v5SEqHHuioGX73cVJ3jYZ0BA+QHVcQ/w47BelxjkcMTnBKBulRK4GGmuOiqOaHA4347FjqQ0wNn1zrhqm5+EzZeq9vR6Cj8y/wmpPmbs5PAUMxNZ4U1GpDjF4AKQBDf3+sEN3xvpwprvKk3/VLL6chAJBZLEaTaipUvNYtWBvkLJqlYnXdxlFZ7hggzutseveIpgKI/gMITVNO2xGjwujHoDRMWdT32uHKzyUlh7BQUgdfUxlX+IWZxQnA1k4DKrIZC3r+akiahlLjNXU1tfW");
        private static int[] order = new int[] { 13,11,13,11,10,11,7,11,8,12,12,12,13,13,14 };
        private static int key = 215;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
