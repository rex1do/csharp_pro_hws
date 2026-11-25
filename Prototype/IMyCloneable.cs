namespace Prototype
{
    public interface IMyCloneable<out T>
    {
        T MyClone();
    }
}
