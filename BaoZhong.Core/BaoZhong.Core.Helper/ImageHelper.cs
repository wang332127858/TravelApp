using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace BaoZhong.Core.Helper
{
    /// <summary>
    /// desc:Õº∆¨∏®÷˙¿‡
    /// author:cgm
    /// </summary>
	public class ImageHelper
	{
		private static bool _isloadjpegcodec = false;

		private static ImageCodecInfo _jpegcodec = null;

		public static ImageCodecInfo GetJPEGCodec()
		{
			ImageCodecInfo jpegcodec;
			if (ImageHelper._isloadjpegcodec)
			{
				jpegcodec = ImageHelper._jpegcodec;
			}
			else
			{
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo[] array = imageEncoders;
				for (int i = 0; i < array.Length; i++)
				{
					ImageCodecInfo imageCodecInfo = array[i];
					if (imageCodecInfo.MimeType.IndexOf("jpeg") > -1)
					{
						ImageHelper._jpegcodec = imageCodecInfo;
						break;
					}
				}
				ImageHelper._isloadjpegcodec = true;
				jpegcodec = ImageHelper._jpegcodec;
			}
			return jpegcodec;
		}

		public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
		{
			Image image = Image.FromFile(sourceFilename);
			if (image.Width <= width && image.Height <= height)
			{
				File.Copy(sourceFilename, destFilename, true);
				image.Dispose();
			}
			else
			{
				int width2 = image.Width;
				int height2 = image.Height;
				float num = (float)height / (float)height2;
				if ((float)width / (float)width2 < num)
				{
					num = (float)width / (float)width2;
				}
				width = (int)((float)width2 * num);
				height = (int)((float)height2 * num);
				Image image2 = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(image2);
				graphics.Clear(Color.White);
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, width2, height2), GraphicsUnit.Pixel);
				EncoderParameters encoderParameters = new EncoderParameters();
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 100L);
				encoderParameters.Param[0] = encoderParameter;
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo encoder = null;
				for (int i = 0; i < imageEncoders.Length; i++)
				{
					if (imageEncoders[i].FormatDescription.Equals("JPEG"))
					{
						encoder = imageEncoders[i];
						break;
					}
				}
				image2.Save(destFilename, encoder, encoderParameters);
				encoderParameters.Dispose();
				encoderParameter.Dispose();
				image.Dispose();
				image2.Dispose();
				graphics.Dispose();
			}
		}

		public static void GenerateImageWatermark(string originalPath, string watermarkPath, string targetPath, int position, int opacity, int quality)
		{
			Image image = null;
			Image image2 = null;
			ImageAttributes imageAttributes = null;
			Graphics graphics = null;
			try
			{
				image = Image.FromFile(originalPath);
				image2 = new Bitmap(watermarkPath);
				if (image2.Height >= image.Height || image2.Width >= image.Width)
				{
					image.Save(targetPath);
				}
				else
				{
					if (quality < 0 || quality > 100)
					{
						quality = 80;
					}
					float num;
					if (opacity > 0 && opacity <= 10)
					{
						num = (float)opacity / 10f;
					}
					else
					{
						num = 0.5f;
					}
					int x = 0;
					int y = 0;
					switch (position)
					{
					case 1:
						x = (int)((float)image.Width * 0.01f);
						y = (int)((float)image.Height * 0.01f);
						break;
					case 2:
						x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
						y = (int)((float)image.Height * 0.01f);
						break;
					case 3:
						x = (int)((float)image.Width * 0.99f - (float)image2.Width);
						y = (int)((float)image.Height * 0.01f);
						break;
					case 4:
						x = (int)((float)image.Width * 0.01f);
						y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
						break;
					case 5:
						x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
						y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
						break;
					case 6:
						x = (int)((float)image.Width * 0.99f - (float)image2.Width);
						y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
						break;
					case 7:
						x = (int)((float)image.Width * 0.01f);
						y = (int)((float)image.Height * 0.99f - (float)image2.Height);
						break;
					case 8:
						x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
						y = (int)((float)image.Height * 0.99f - (float)image2.Height);
						break;
					case 9:
						x = (int)((float)image.Width * 0.99f - (float)image2.Width);
						y = (int)((float)image.Height * 0.99f - (float)image2.Height);
						break;
					}
					ColorMap[] map = new ColorMap[]
					{
						new ColorMap
						{
							OldColor = Color.FromArgb(255, 0, 255, 0),
							NewColor = Color.FromArgb(0, 0, 0, 0)
						}
					};
					float[][] array = new float[5][];
					float[][] arg_2DE_0 = array;
					int arg_2DE_1 = 0;
					float[] array2 = new float[5];
					array2[0] = 1f;
					arg_2DE_0[arg_2DE_1] = array2;
					float[][] arg_2F5_0 = array;
					int arg_2F5_1 = 1;
					array2 = new float[5];
					array2[1] = 1f;
					arg_2F5_0[arg_2F5_1] = array2;
					float[][] arg_30C_0 = array;
					int arg_30C_1 = 2;
					array2 = new float[5];
					array2[2] = 1f;
					arg_30C_0[arg_30C_1] = array2;
					float[][] arg_320_0 = array;
					int arg_320_1 = 3;
					array2 = new float[5];
					array2[3] = num;
					arg_320_0[arg_320_1] = array2;
					array[4] = new float[]
					{
						0f,
						0f,
						0f,
						0f,
						1f
					};
					float[][] newColorMatrix = array;
					ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
					imageAttributes = new ImageAttributes();
					imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);
					imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
					graphics = Graphics.FromImage(image);
					graphics.DrawImage(image2, new Rectangle(x, y, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel, imageAttributes);
					EncoderParameters encoderParameters = new EncoderParameters();
					encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, new long[]
					{
						(long)quality
					});
					if (ImageHelper.GetJPEGCodec() != null)
					{
						image.Save(targetPath, ImageHelper._jpegcodec, encoderParameters);
					}
					else
					{
						image.Save(targetPath);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (imageAttributes != null)
				{
					imageAttributes.Dispose();
				}
				if (image2 != null)
				{
					image2.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
			}
		}

		public static void GenerateTextWatermark(string originalPath, string targetPath, string text, int textSize, string textFont, int position, int quality)
		{
			Image image = null;
			Graphics graphics = null;
			try
			{
				image = Image.FromFile(originalPath);
				graphics = Graphics.FromImage(image);
				if (quality < 0 || quality > 100)
				{
					quality = 80;
				}
				Font font = new Font(textFont, (float)textSize, FontStyle.Regular, GraphicsUnit.Pixel);
				SizeF sizeF = graphics.MeasureString(text, font);
				float num = 0f;
				float num2 = 0f;
				switch (position)
				{
				case 1:
					num = (float)image.Width * 0.01f;
					num2 = (float)image.Height * 0.01f;
					break;
				case 2:
					num = (float)image.Width * 0.5f - sizeF.Width / 2f;
					num2 = (float)image.Height * 0.01f;
					break;
				case 3:
					num = (float)image.Width * 0.99f - sizeF.Width;
					num2 = (float)image.Height * 0.01f;
					break;
				case 4:
					num = (float)image.Width * 0.01f;
					num2 = (float)image.Height * 0.5f - sizeF.Height / 2f;
					break;
				case 5:
					num = (float)image.Width * 0.5f - sizeF.Width / 2f;
					num2 = (float)image.Height * 0.5f - sizeF.Height / 2f;
					break;
				case 6:
					num = (float)image.Width * 0.99f - sizeF.Width;
					num2 = (float)image.Height * 0.5f - sizeF.Height / 2f;
					break;
				case 7:
					num = (float)image.Width * 0.01f;
					num2 = (float)image.Height * 0.99f - sizeF.Height;
					break;
				case 8:
					num = (float)image.Width * 0.5f - sizeF.Width / 2f;
					num2 = (float)image.Height * 0.99f - sizeF.Height;
					break;
				case 9:
					num = (float)image.Width * 0.99f - sizeF.Width;
					num2 = (float)image.Height * 0.99f - sizeF.Height;
					break;
				}
				graphics.DrawString(text, font, new SolidBrush(Color.White), num + 1f, num2 + 1f);
				graphics.DrawString(text, font, new SolidBrush(Color.Black), num, num2);
				EncoderParameters encoderParameters = new EncoderParameters();
				encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, new long[]
				{
					(long)quality
				});
				if (ImageHelper.GetJPEGCodec() != null)
				{
					image.Save(targetPath, ImageHelper._jpegcodec, encoderParameters);
				}
				else
				{
					image.Save(targetPath);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
			}
		}

		public static MemoryStream GenerateCheckCode(out string checkCode)
		{
			checkCode = string.Empty;
			Color color = ColorTranslator.FromHtml("#1AE61A");
			char[] array = new char[]
			{
				'2',
				'3',
				'4',
				'5',
				'6',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F',
				'G',
				'H',
				'J',
				'K',
				'L',
				'M',
				'N',
				'P',
				'R',
				'S',
				'T',
				'W',
				'X',
				'Y'
			};
			Random random = new Random();
			for (int i = 0; i < 4; i++)
			{
				checkCode += array[random.Next(array.Length)];
			}
			int width = 85;
			Bitmap bitmap = new Bitmap(width, 30);
			Graphics graphics = Graphics.FromImage(bitmap);
			Random random2 = new Random(DateTime.Now.Millisecond);
			Brush brush = new SolidBrush(Color.FromArgb(4683611));
			graphics.Clear(ColorTranslator.FromHtml("#EBFDDF"));
			using (StringFormat stringFormat = new StringFormat())
			{
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				stringFormat.FormatFlags = StringFormatFlags.NoWrap;
				Matrix matrix = new Matrix();
				float num = -25f;
				float num2 = 0f;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				for (int i = 0; i < checkCode.Length; i++)
				{
					int num3 = random2.Next(20, 24);
					Font font = ImageHelper.CreateFont(IOHelper.GetMapPath("/fonts/checkCode.ttf"), (float)num3, FontStyle.Bold, GraphicsUnit.Point, 0);
					SizeF sizeF = graphics.MeasureString(checkCode[i].ToString(), font);
					matrix.RotateAt((float)random2.Next(-15, 10), new PointF(num + sizeF.Width / 2f, num2 + sizeF.Height / 2f));
					graphics.Transform = matrix;
					graphics.DrawString(checkCode[i].ToString(), font, Brushes.Green, new RectangleF(num, num2, (float)bitmap.Width, (float)bitmap.Height), stringFormat);
					num += sizeF.Width * 3f / 5f;
					num2 += 0f;
					graphics.RotateTransform(0f);
					matrix.Reset();
					font.Dispose();
				}
			}
			Pen pen = new Pen(Color.Black, 0f);
			MemoryStream memoryStream = new MemoryStream();
			MemoryStream result;
			try
			{
				bitmap.Save(memoryStream, ImageFormat.Png);
				result = memoryStream;
			}
			finally
			{
				bitmap.Dispose();
				graphics.Dispose();
			}
			return result;
		}

		public static Font CreateFont(string fontFile, float fontSize, FontStyle fontStyle, GraphicsUnit graphicsUnit, byte gdiCharSet)
		{
			PrivateFontCollection privateFontCollection = new PrivateFontCollection();
			privateFontCollection.AddFontFile(fontFile);
			return new Font(privateFontCollection.Families[0], fontSize, fontStyle, graphicsUnit, gdiCharSet);
		}

		public static void TranserImageFormat(string originalImagePath, string newFormatImagePath, ImageFormat fortmat)
		{
			Bitmap bitmap = new Bitmap(originalImagePath);
			bitmap.Save(newFormatImagePath, ImageFormat.Jpeg);
		}
	}
}
