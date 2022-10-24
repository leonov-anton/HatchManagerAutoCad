using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.ApplicationServices;

namespace HatchManagerAutoCad
{
    public class HatchManager
    {
        [CommandMethod("HMAN")]
        static public void CreateBlockTable()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            CivilDocument cdoc = CivilDocument.GetCivilDocument(db);

        }
    }
}
