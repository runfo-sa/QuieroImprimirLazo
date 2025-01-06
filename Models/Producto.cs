namespace QuieroLazos.Models;

public record struct Producto(string Nombre, string Traduccion)
{
    public Producto(string nombre) : this(nombre, nombre) { }
}