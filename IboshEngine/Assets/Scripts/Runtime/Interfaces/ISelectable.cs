namespace IboshEngine.Runtime.Interfaces
{
    public interface ISelectable
    {
        public bool IsSelectable { get; set; }
        void Select();
        void Deselect();
        void SetSelectability(bool isSelectable);
    }
}