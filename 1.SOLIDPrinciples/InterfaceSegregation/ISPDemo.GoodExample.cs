namespace _1.SOLIDPrinciples.InterfaceSegregation
{
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public class PhotoPrinter : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    interface IMultiFunctionDevice : IPrinter, IScanner
    {

    }

    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        private IScanner _scanner;
        private IPrinter _printer;

        public MultiFunctionMachine(IScanner scanner, IPrinter printer)
        {
            _scanner = scanner;
            _printer = printer;
        }

        public void Print(Document d)
        {
            _printer.Print(d);
        }

        public void Scan(Document d)
        {
            _scanner.Scan(d);
        }
    }

    class ISPDemoGood
    {
    }
}
