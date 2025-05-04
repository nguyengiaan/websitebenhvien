namespace websitebenhvien.Models.EnitityVM
{
    public class Summenote
    {
        public Summenote(string iDEditor,bool loadlibraty=true) 
        {
            this.IDEditor = iDEditor;
            this.Loadlibraty = loadlibraty;

        }
        public string IDEditor { get; set; }

        public bool Loadlibraty { get; set; }

        public int Height { get; set; } = 300;
        public string Toolbar { get; set; } = @"[
            [""style"", [""style"", ""bold"", ""italic"", ""underline"", ""clear""]],
            [""font"", [""strikethrough"", ""superscript"", ""subscript"", ""fontname""]],
            [""fontsize"", [""fontsize""]],
            [""color"", [""color"", ""forecolor"", ""backcolor""]],
            [""para"", [""ul"", ""ol"", ""paragraph"", ""floatLeft"", ""floatNone"", ""floatRight""]],
            [""height"", [""height""]],
            [""insert"", [""link"", ""video"", ""table"", ""imageManager""]],
            [""misc"", [""codeview"", ""fullscreen"", ""help""]]
        ]";
    }
}
