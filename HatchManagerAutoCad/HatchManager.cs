using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Colors;
using Autodesk.Gis.Map;
using ODTables = Autodesk.Gis.Map.ObjectData.Tables;
using ODTable = Autodesk.Gis.Map.ObjectData.Table;
using System;
using System.Collections.Generic;
using Autodesk.Gis.Map.ObjectData;
using Entity = Autodesk.AutoCAD.DatabaseServices.Entity;
using System.Linq;
using System.Collections;
using DataBase;
using System.IO;

namespace HatchManagerAutoCad
{
    public class HatchManager
    {
        private static Document doc = Application.DocumentManager.MdiActiveDocument;
        private static Database db = doc.Database;
        private static Editor ed = doc.Editor;
        private static MapApplication map = HostMapApplicationServices.Application;

        Sqliter db_hatch = new Sqliter();

        public HatchManager(ArrayList hatchAtr)
        {
            this.HatchName = (string)hatchAtr[0];
            this.HatchPatt = (string)hatchAtr[1];
            this.HatchDescr = (string)hatchAtr[2];
            this.HatchAngle = (long)hatchAtr[4];
            this.HatchScale = (double)hatchAtr[3];
            this.HatchLayer = (string)hatchAtr[5];
            this.HatchColor = (string)hatchAtr[6];
            this.HatchColorBack = (string)hatchAtr[7];
            this.HatchTransp = (long)hatchAtr[8];
            this.ODTableName = (string)hatchAtr[9];
            this.ODTableFieldsName = (string)hatchAtr[10];
            this.ODTableFieldType = (string)hatchAtr[11];
            this.ODTableFieldSorce = (string)hatchAtr[12];
        }

        private string HatchName { get; set; }
        private string HatchPatt { get; set; }
        private string HatchDescr { get; set; }
        private double HatchScale { get; set; }
        private long HatchAngle { get; set; }
        private string HatchLayer { get; set; }
        private string HatchColor { get; set; }
        private string HatchColorBack { get; set; }
        private long HatchTransp { get; set; }
        private string ODTableName { get; set; }
        private string ODTableFieldsName { get; set; }
        private string ODTableFieldType { get; set; }
        private string ODTableFieldSorce { get; set; }


        [CommandMethod("SMLNIS", "HMAN", CommandFlags.Modal)]
        static public void CreateBlockTable()
        {
            HatchManagerGUI hatchMan = new HatchManagerGUI();
            try
            {
                UploadCountDB.UploadDB(true, Path.GetFileName(doc.Name), "Менеждер штриховок", "General");
            }
            catch { }
            Application.ShowModelessDialog(hatchMan);
        }

        // Создание новой штриховки
        public void CreateNewHatch()
        {
            // Выбор режима создания
            PromptKeywordOptions pko = new PromptKeywordOptions("\nСоздать штриховку по точке или контуру:");
            pko.Keywords.Add("Точка");
            pko.Keywords.Add("Контур");
            PromptResult pr = ed.GetKeywords(pko);
            if (pr.Status != PromptStatus.OK)
            {
                ed.WriteMessage("\nВыполнение прервано");
                return;
            }
            try
            {
                using (DocumentLock docLock = doc.LockDocument())
                {
                    using (Transaction t = db.TransactionManager.StartTransaction())
                    {
                        // Создание штриховки
                        Hatch newHatchObj = new Hatch();

                        BlockTableRecord btr = (BlockTableRecord)t.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                        btr.AppendEntity(newHatchObj);
                        t.AddNewlyCreatedDBObject(newHatchObj, true);
                        
                        // Назначение атребутов штриховки
                        SetHatchProperty(newHatchObj);

                        // Создание коллекции id объектов для задания по ним контура
                        ObjectIdCollection hatBounObjIdCol = new ObjectIdCollection();

                        bool stopLoop = false;

                        if (pr.StringResult == "Контур")
                        {
                            while (stopLoop == false)
                            {
                                PromptSelectionOptions pso = new PromptSelectionOptions();
                                pso.MessageForAdding = "\nВыберите объект с замкнутым контуром";
                                pso.SingleOnly = true;
                                PromptSelectionResult psr = ed.GetSelection(pso);
                                if (psr.Status != PromptStatus.OK)
                                {
                                    ed.WriteMessage("\nВыполнение прервано");
                                    stopLoop = true;
                                }
                                else
                                {
                                    foreach (ObjectId boundObjId in psr.Value.GetObjectIds())
                                        hatBounObjIdCol.Add(boundObjId);
                                    try
                                    {
                                        newHatchObj.AppendLoop(HatchLoopTypes.External, hatBounObjIdCol); // Назначение контура штриховки
                                        hatBounObjIdCol.Clear(); // Очистка списка границ
                                    }
                                    catch
                                    {
                                        ed.WriteMessage("n\nОшибка! Не удалось определить замкнутый контур!");
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            while (stopLoop == false)
                            {
                                PromptPointOptions ppo = new PromptPointOptions("\nУкажите точку внутри замкнутого контура:");
                                PromptPointResult ppr = ed.GetPoint(ppo);
                                if (ppr.Status != PromptStatus.OK)
                                {
                                    ed.WriteMessage("\nВыполнение прервано");
                                    stopLoop = true;
                                }
                                else
                                {
                                    // Создание замкнутого контура по точке
                                    DBObjectCollection boundObjs = ed.TraceBoundary(ppr.Value, true);
                                    if (boundObjs.Count == 0)
                                    {
                                        ed.WriteMessage("n\nОшибка! Не удалось определить замкнутый контур!");
                                        return;
                                    }

                                    ObjectId boundObjId = btr.AppendEntity((Entity)boundObjs[0]);
                                    t.AddNewlyCreatedDBObject(boundObjs[0], true);
                                    hatBounObjIdCol.Add(boundObjId);

                                    newHatchObj.AppendLoop(HatchLoopTypes.External, hatBounObjIdCol); // Назначение контура штриховки
                                    boundObjs[0].Erase(); // удаление полилинии контура после создания штриховки
                                }
                            }
                        }

                        // Проверка задан ли хоть один контур, если нет выйти без записи изменений
                        if (newHatchObj.NumberOfLoops == 0)
                        {
                            return;
                        }

                        // Назначение таблицы ObjectData штриховки
                        newHatchObj.EvaluateHatch(true);
                        WriteObjectData(newHatchObj);
                        ed.Regen();
                        t.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                db_hatch.errorsLogging(DateTime.Now.ToString(), Environment.UserName, ex.Message);
                Application.ShowAlertDialog("ОШИБКА! Сообщите координатору о проблемме!");
            }
        }

        // Изменение штриховки 
        public void ChangeHatch()
        {
            List<TypedValue> typedValueList = new List<TypedValue>();
            typedValueList.Add(new TypedValue(0, "HATCH"));
            SelectionFilter selectionFilter = new SelectionFilter(typedValueList.ToArray());
            PromptSelectionResult psr = ed.GetSelection(selectionFilter);
            if (psr.Status != PromptStatus.OK)
            {
                ed.WriteMessage("\nВыполнение прервано");
                return;
            }
            try
            {
                using (DocumentLock docLock = doc.LockDocument())
                {
                    using (Transaction t = db.TransactionManager.StartTransaction())
                    {
                        foreach (ObjectId objectId in psr.Value.GetObjectIds())
                        {
                            Hatch hatchObj = (Hatch)t.GetObject(objectId, OpenMode.ForWrite);
                            SetHatchProperty(hatchObj);
                            hatchObj.EvaluateHatch(true);
                            WriteObjectData(hatchObj);
                        }
                        ed.Regen();
                        t.Commit();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                db_hatch.errorsLogging(DateTime.Now.ToString(), Environment.UserName, ex.Message);
                Application.ShowAlertDialog("ОШИБКА! Сообщите координатору о проблемме!");
            }
        }

        // Назначение таблицы ObjectData
        public void SetOdataTable()
        {
            List<TypedValue> typedValueList = new List<TypedValue>();
            typedValueList.Add(new TypedValue(0, "HATCH"));
            SelectionFilter selectionFilter = new SelectionFilter(typedValueList.ToArray());
            PromptSelectionResult psr = ed.GetSelection(selectionFilter);
            if (psr.Status != PromptStatus.OK)
            {
                ed.WriteMessage("\nВыполнение прервано");
                return;
            }
            try
            {
                using (DocumentLock docLock = doc.LockDocument())
                {
                    using (Transaction t = db.TransactionManager.StartTransaction())
                    {
                        foreach (ObjectId objectId in psr.Value.GetObjectIds())
                        {
                            Hatch hatchObj = (Hatch)t.GetObject(objectId, OpenMode.ForWrite);
                            WriteObjectData(hatchObj);
                        }
                        t.Commit();
                        ed.Regen();
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                db_hatch.errorsLogging(DateTime.Now.ToString(), Environment.UserName, $"{ex.Source}\n{ex.Message}");
                Application.ShowAlertDialog("ОШИБКА! Сообщите координатору о проблемме!");
            }
        }

        // Назначение атребутов штриховки
        private void SetHatchProperty(Hatch hatchObj)
        {
            using (Transaction trn = db.TransactionManager.StartTransaction())
            {
                hatchObj.SetHatchPattern(HatchPatternType.PreDefined, HatchPatt);
                hatchObj.PatternScale = HatchScale;

                // Цвет штриховки
                if (string.IsNullOrEmpty(HatchColor))
                    hatchObj.Color = Color.FromColorIndex(ColorMethod.ByLayer, 256);
                else if (HatchColor.Contains(","))
                {
                    string[] RGB = HatchColor.Split(',');
                    hatchObj.Color = Color.FromRgb(byte.Parse(RGB[0]), byte.Parse(RGB[1]), byte.Parse(RGB[2]));
                }
                else
                    hatchObj.Color = Color.FromColorIndex(ColorMethod.ByAci, short.Parse(HatchColor));

                // Цвет фона штриховки
                if (string.IsNullOrEmpty(HatchColorBack))
                    hatchObj.BackgroundColor = Color.FromColorIndex(ColorMethod.ByLayer, 256);
                else if (HatchColorBack.Contains(","))
                {
                    string[] RGB = HatchColorBack.Split(',');
                    hatchObj.BackgroundColor = Color.FromRgb(byte.Parse(RGB[0]), byte.Parse(RGB[1]), byte.Parse(RGB[2]));
                }
                else
                    hatchObj.BackgroundColor = Color.FromColorIndex(ColorMethod.ByAci, short.Parse(HatchColorBack));

                // Проверка есть ли слой, если нет создает
                LayerTable lt = (LayerTable)trn.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (!lt.Has(HatchLayer))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = HatchLayer;
                    lt.UpgradeOpen();
                    lt.Add(ltr);
                    lt.DowngradeOpen();
                    trn.AddNewlyCreatedDBObject(ltr, true);
                }
                // Слой штриховки
                hatchObj.Layer = HatchLayer;

                // Прозрачность штриховки
                if (HatchTransp == 0)
                {
                    byte alpha = 0;
                    hatchObj.Transparency = new Transparency(alpha);
                }
                else
                {
                    byte alpfa = (byte)(225 * (100 - HatchTransp) / 100);
                    hatchObj.Transparency = new Transparency(alpfa);
                }

                // Угол поворота штриховки
                hatchObj.PatternAngle = (Math.PI * HatchAngle) / 180;
                trn.Commit();
                return;
            }
        }

        // Запись ObjectData
        private void WriteObjectData(Hatch hatchObj)
        {
            ODTable table = GetOrCreateODTable();
            ODTables tables = map.ActiveProject.ODTables; // таблицы обжект дата в файле
            Records objectRecs = tables.GetObjectRecords(0, hatchObj.ObjectId, Autodesk.Gis.Map.Constants.OpenMode.OpenForWrite, true); // записи таблиц обжект даты присоединенные к объекту
            // Удаление ранее присоединенных таблиц
            if (objectRecs.Count > 0)
            {
                IEnumerator iter = objectRecs.GetEnumerator();
                iter.MoveNext();
                objectRecs.RemoveRecord();
                objectRecs.Dispose();
            }
            // Назначение таблици
            Record rec = Record.Create();
            table.InitRecord(rec);
            string[] filedNames = ODTableFieldSorce.Split('~');
            string[] fieldDataTypes = ODTableFieldType.Split('~');
            for (byte i = 0; i < filedNames.Count(); i++) 
            {
                if (fieldDataTypes[i] == "Int")
                {
                    int value = db_hatch.getIntObjData(filedNames[i], ODTableName, HatchName);
                    rec[i].Assign(value);
                }
                else if (fieldDataTypes[i] == "Real")
                {
                    double value = db_hatch.getRealObjData(filedNames[i], ODTableName, HatchName);
                    rec[i].Assign(value);
                }
                else
                {
                    string value = db_hatch.getStrObjData(filedNames[i], ODTableName, HatchName);
                    rec[i].Assign(value);
                }
            }
            table.AddRecord(rec, hatchObj.ObjectId);
        }

        // Получение или создание таблицы ObjectData по названию
        private ODTable GetOrCreateODTable()
        {
            ODTables tables = map.ActiveProject.ODTables;
            // Попытка получения таблицы по имени
            try
            {
                ODTable table = tables[ODTableName];
                return table;
            }
            // Создание новой если таблица не найдена
            catch (MapException)
            {
                FieldDefinitions fieldDefs = map.ActiveProject.MapUtility.NewODFieldDefinitions();
                string[] filedNames = ODTableFieldsName.Split('~');
                string[] fieldDataTypes = ODTableFieldType.Split('~');
                for (byte i = 0; i < filedNames.Count(); i++)
                {
                    if (fieldDataTypes[i] == "Int")
                        fieldDefs.Add(filedNames[i], "", Autodesk.Gis.Map.Constants.DataType.Integer, i);
                    else if (fieldDataTypes[i] == "Real")
                        fieldDefs.Add(filedNames[i], "", Autodesk.Gis.Map.Constants.DataType.Real, i);
                    else
                        fieldDefs.Add(filedNames[i], "", Autodesk.Gis.Map.Constants.DataType.Character, i);
                }
                tables.Add(ODTableName, fieldDefs, "", true);
                ODTable table = tables[ODTableName];
                return table;
            }
        }
    }
}
