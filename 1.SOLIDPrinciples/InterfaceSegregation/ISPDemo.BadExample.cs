namespace _1.SOLIDPrinciples.InterfaceSegregation
{
    public class Document
    {

    }

    // Its fine if we work with a multifunction printer
    public interface IMachine
    {
        void PrintDocument(Document d);
        void ScanDocument(Document d);
        void FaxDocument(Document d);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void FaxDocument(Document d)
        {
            //
        }

        public void PrintDocument(Document d)
        {
            //
        }

        public void ScanDocument(Document d)
        {
            //
        }
    }

    // Implements functions that we don't need
    public class OldFashionPrinter : IMachine
    {
        public void FaxDocument(Document d)
        {
            //
        }

        public void PrintDocument(Document d)
        {
            throw new System.NotImplementedException();
        }

        public void ScanDocument(Document d)
        {
            throw new System.NotImplementedException();
        }
    }

    class ISPDemoBad
    {
    }
}
