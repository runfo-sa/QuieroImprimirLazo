namespace QuieroLazos.Models
{
    public struct State(int total)
    {
        public int Total { get; set; } = total;
        public int Pending { get; private set; } = total;
        public int Complete { get; set; } = 0;

        public override readonly string ToString() => $"Total de Tropas: {Total} | Pendientes: {Pending} | Completadas: {Complete}";

        public static State operator ++(State state)
        {
            state.Complete++;
            state.Pending = state.Total - state.Complete;
            return state;
        }
    }
}
