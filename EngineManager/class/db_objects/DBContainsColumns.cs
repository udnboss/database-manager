using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Serialization;

namespace EngineManager
{
    public class DBContainsColumns : DBSchemaObject
    {
        private List<string> lookup_tooltip_columns = new List<string>();
        [Editor(typeof(MultiSelectEditor), typeof(UITypeEditor))]
        [Category("Lookup Options"), Description("Additional columns to show in the tooltip when displayed for the lookup value")]
        public List<string> LookupTooltipColumns
        {
            get { return lookup_tooltip_columns; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "lookup_tooltip_columns", "-", string.Join(", ", value));
                }

                lookup_tooltip_columns = value;
            }
        }

        private DBColumn row_badge;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowBadge
        {
            get { return row_badge; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_badge", this.row_badge == null ? "" : this.row_badge.Name, value == null ? "" : value.Name);
                }

                row_badge = value;
            }
        }

        private DBColumn row_subtitle;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowSubTitle
        {
            get { return row_subtitle; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_subtitle", this.row_subtitle == null ? "" : this.row_subtitle.Name, value == null ? "" : value.Name);
                }

                row_subtitle = value;
            }
        }

        private DBColumn row_date;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowDate
        {
            get { return row_date; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_date", this.row_date == null ? "" : this.row_date.Name, value == null ? "" : value.Name);
                }

                row_date = value;
            }
        }

        private DBColumn row_iconclass;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowIconClass
        {
            get { return row_iconclass; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_iconclass", this.row_iconclass == null ? "" : this.row_iconclass.Name, value == null ? "" : value.Name);
                }

                row_iconclass = value;
            }
        }

        private DBColumn row_image;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowImage
        {
            get { return row_image; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_image", this.row_image == null ? "" : this.row_image.Name, value == null ? "" : value.Name);
                }

                row_image = value;
            }
        }

        private DBColumn row_labeltype;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowLabelType
        {
            get { return row_labeltype; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_labeltype", this.row_labeltype == null ? "" : this.row_labeltype.Name, value == null ? "" : value.Name);
                }

                row_labeltype = value;
            }
        }

        private DBColumn row_color;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowColor
        {
            get { return row_color; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_color", this.row_color == null ? "" : this.row_color.Name, value == null ? "" : value.Name);
                }

                row_color = value;
            }
        }

        private DBColumn visible_subforms;
        [Category("Data"), TypeConverter(typeof(ColumnListTypeConverter))]
        [Description("Defines which column provides the list of visible subforms as semicolon separated table names.")]
        public DBColumn VisibleSubForms
        {
            get { return visible_subforms; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "visible_subforms", this.visible_subforms == null ? "" : this.visible_subforms.Name, value == null ? "" : value.Name);
                }

                visible_subforms = value;
            }
        }

        private DBColumn row_name;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowName
        {
            get { return row_name; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_name", this.row_name == null ? "" : this.row_name.Name, value == null ? "" : value.Name);
                }

                row_name = value;
            }
        }

        private DBColumn row_title;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowTitle
        {
            get { return row_title; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_title", this.row_title == null ? "" : this.row_title.Name, value == null ? "" : value.Name);
                }

                row_title = value;
            }
        }

        private DBColumn row_description;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowDescription
        {
            get { return row_description; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_description", this.row_description == null ? "" : this.row_description.Name, value == null ? "" : value.Name);
                }

                row_description = value;
            }
        }

        private DBColumn row_group;
        [Category("Lookup Options"), TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn RowGroup
        {
            get { return row_group; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "row_group", this.row_group == null ? "" : this.row_group.Name, value == null ? "" : value.Name);
                }

                row_group = value;
            }
        }

        private string title;
        [DataMember, Category("Appearance"), Description("Custom Table Title")]
        public string Title
        {
            get { return title; }
            set
            {
                //must not be initial
                if (this.State != DBObjectState.None)
                {
                    DBObjectManager.CreateExtendedProperty(this, "title", this.title, value);
                }

                title = value;
            }
        }

        public string GetSelect()
        {
            var statement = string.Format("select \r\n\t{0}\r\nfrom\r\n\t{1}\r\nwhere\r\n\t1 = 1",
                    string.Join(",\r\n\t", (this as IContainsColumns).Columns.Select(c=>c.FullName).ToList()),
                    this.FullName);

            return statement;
        }

        public string GetInsert()
        {
            var statement = string.Format("insert into {0}\\r\nt({1})\r\nvalues\r\n\t({2})",
                    this.FullName,
                    string.Join(",\r\n\t", (this as IContainsColumns).Columns.Select(c => c.FullName).ToList()),
                    string.Join(",\r\n\t", (this as IContainsColumns).Columns.Select(c => "'" + c.FullName + "'").ToList())
                    );

            return statement;
        }

        public string GetUpdate()
        {
            var statement = string.Format("update {0}\r\nset\r\n\t{1}\r\nwhere\r\n\t0 = 1",
                    this.FullName,
                    string.Join(",\r\n\t", (this as IContainsColumns).Columns.Select(c => c.FullName + " = ''" ).ToList())
                    );

            return statement;
        }

        public string GetDelete()
        {
            var statement = string.Format("delete from\r\n\t{0}\r\nwhere\r\n\t0 = 1", this.FullName);

            return statement;
        }
    }
}
