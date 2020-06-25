namespace Assets.Scripts.HeightMapProviders
{
    public interface IHeightMapProvider
    {
        float[,] Generate(int power, float roughness);
    }
}
