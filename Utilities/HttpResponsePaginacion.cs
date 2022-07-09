using WebApiKalum.Dtos;

namespace WebApiKalum.Utilities
{
    public class HttpResponsePaginacion<T> : PaginacionDTO<T>
    {
        public HttpResponsePaginacion(IQueryable<T> source, int number)
        {
            this.Number = number;
            int CantidadRegistrosPorPagina = 5;
            int TotalRegistros = source.Count();
            this.TotalPages = (int)Math.Ceiling((Double)TotalRegistros / CantidadRegistrosPorPagina);
            this.Content = source.Skip(CantidadRegistrosPorPagina * Number).Take(CantidadRegistrosPorPagina).ToList();
            if(this.Number == 0)
            {
                this.First = true;
            }
            else if((this.Number + 1) == this.TotalPages)
            {
                this.Last = true;
            }
        }
    }
}