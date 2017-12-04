using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;

namespace PhotoMosaic.App
{
    internal class DataAccess : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public DataAccess(string path)
        {
            string dbLocation = Path.Combine(path, "db.sqlite");
            bool shouldCreateSchema = false;

            if (!File.Exists(dbLocation))
            {
                SQLiteConnection.CreateFile(dbLocation);
                shouldCreateSchema = true;
            }

            _connection = new SQLiteConnection("Data Source=" + dbLocation);
            _connection.Open();

            if (shouldCreateSchema)
            {
                CreateSchema();
            }
        }

        public void SaveSourceImageInfo(IEnumerable<ImageInfo> images)
        {
            string sql = @"insert into source_image (id, extension, file_path, alpha, red, green, blue)
                           values (@id, @extension, @file_path, @alpha, @red, @green, @blue)";

            SQLiteParameter idParameter = new SQLiteParameter {ParameterName = "id"};
            SQLiteParameter extensionParameter = new SQLiteParameter {ParameterName = "extension"};
            SQLiteParameter filePathParameter = new SQLiteParameter {ParameterName = "file_path"};
            SQLiteParameter alphaParameter = new SQLiteParameter {ParameterName = "alpha"};
            SQLiteParameter redParameter = new SQLiteParameter {ParameterName = "red"};
            SQLiteParameter greenParameter = new SQLiteParameter {ParameterName = "green"};
            SQLiteParameter blueParameter = new SQLiteParameter {ParameterName = "blue"};

            using (var transaction = _connection.BeginTransaction())
            {
                using (var command = new SQLiteCommand(sql, _connection))
                {
                    command.Parameters.Add(idParameter);
                    command.Parameters.Add(extensionParameter);
                    command.Parameters.Add(filePathParameter);
                    command.Parameters.Add(alphaParameter);
                    command.Parameters.Add(redParameter);
                    command.Parameters.Add(greenParameter);
                    command.Parameters.Add(blueParameter);

                    foreach (var image in images)
                    {
                        idParameter.Value = image.Id.ToString();
                        extensionParameter.Value = image.Extension;
                        filePathParameter.Value = image.Path;
                        alphaParameter.Value = image.Color.A;
                        redParameter.Value = image.Color.R;
                        greenParameter.Value = image.Color.G;
                        blueParameter.Value = image.Color.B;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public long SaveTargetImage(string filename, byte[] contents)
        {
            const string sql = @"
insert into target_image (extension, image) values (@extension, @image);
select last_insert_rowid()";

            FileInfo fileInfo = new FileInfo(filename);
            long rowId;

            SQLiteParameter extensionParameter = new SQLiteParameter {ParameterName = "@extension", Value = fileInfo.Extension};
            SQLiteParameter imageParameter = new SQLiteParameter {ParameterName = "@image", Value = contents};

            using (var transaction = _connection.BeginTransaction())
            {
                using (var command = new SQLiteCommand(sql, _connection))
                {
                    command.Parameters.Add(extensionParameter);
                    command.Parameters.Add(imageParameter);
                    rowId = (long)command.ExecuteScalar();
                }

                transaction.Commit();
            }

            return rowId;
        }

        public void SaveTargetImageBlocks(long imageId, List<Block<Color>> blocks)
        {
            const string sql = @"
insert into target_image_blocks (image_id, x, y, width, height, alpha, red, green, blue)
values (@imageId, @x, @y, @width, @height, @alpha, @red, @green, @blue)
";

            SQLiteParameter imageParameter = new SQLiteParameter { ParameterName = "@imageId"},
                xParameter = new SQLiteParameter { ParameterName = "@x" },
                yParameter = new SQLiteParameter { ParameterName = "@y" },
                widthParameter = new SQLiteParameter { ParameterName = "@width" },
                heightParameter = new SQLiteParameter { ParameterName = "@height" },
                alphaParameter = new SQLiteParameter { ParameterName = "@alpha" },
                redParameter = new SQLiteParameter { ParameterName = "@red" },
                greenParameter = new SQLiteParameter { ParameterName = "@green" },
                blueParameter = new SQLiteParameter { ParameterName = "@blue" };

            using (var transaction = _connection.BeginTransaction())
            using (var command = new SQLiteCommand(sql, _connection))
            {
                command.Parameters.Add(imageParameter);
                command.Parameters.Add(xParameter);
                command.Parameters.Add(yParameter);
                command.Parameters.Add(widthParameter);
                command.Parameters.Add(heightParameter);
                command.Parameters.Add(alphaParameter);
                command.Parameters.Add(redParameter);
                command.Parameters.Add(greenParameter);
                command.Parameters.Add(blueParameter);

                foreach (var block in blocks)
                {
                    imageParameter.Value = imageId;
                    xParameter.Value = block.Rect.X;
                    yParameter.Value = block.Rect.Y;
                    widthParameter.Value = block.Rect.Width;
                    heightParameter.Value = block.Rect.Height;
                    alphaParameter.Value = block.Data.A;
                    redParameter.Value = block.Data.R;
                    greenParameter.Value = block.Data.G;
                    blueParameter.Value = block.Data.B;

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        private void CreateSchema()
        {
            string sql = @"
create table source_image (
    id text,
    extension text,
    file_path text,
    alpha integer,
    red integer,
    green integer,
    blue integer
)";

            using (SQLiteCommand command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "create table target_image (extension text, image blob)";

            using (SQLiteCommand command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }

            sql =
                @"
create table target_image_blocks (
    image_id integer,
    x integer,
    y integer,
    width integer,
    height integer,
    alpha integer,
    red integer,
    green integer,
    blue integer
)";

            using (SQLiteCommand command = new SQLiteCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
        }
    }
}
