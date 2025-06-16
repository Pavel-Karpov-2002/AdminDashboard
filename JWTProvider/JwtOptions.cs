namespace JWTProvider
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = "fdgiufdshogvmiufdmcnfsdijcgnslkfdjgoksa";
        public int TimeOfLife { get; set; } = 24;
    }
}
