using UnityEngine;

public class SerializeInterfaceAttribute : PropertyAttribute
{
    public System.Type requiredType { get; private set; }
    /// <summary>
    /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
    /// </summary>
    /// <param name="type">Interface type.</param>
    public SerializeInterfaceAttribute(System.Type type)
    {
        this.requiredType = type;
    }
}