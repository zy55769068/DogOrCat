using System.Net.Mime;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;


string fontFilePath = "Assets/Fonts/SourceHanSansCN.otf";

var image = await Image.LoadAsync<Rgba32>("Assets/test.jpg");
var scorer = new YoloScorer<YoloCocoP5Model>("Assets/yolov5n.onnx");

var predictions = scorer.Predict(image);

// 创建字体
var font = new Font(new FontCollection().Add(fontFilePath), 16);

foreach (var prediction in predictions)
{
    var score = Math.Round(prediction.Score, 2);

    var (x, y) = (prediction.Rectangle.Left - 3, prediction.Rectangle.Top - 23);
    var pen = Pens.DashDot(prediction.Label.Color, 1);

    image.Mutate(a => a.DrawPolygon(pen,
        new PointF(prediction.Rectangle.Left, prediction.Rectangle.Top),
        new PointF(prediction.Rectangle.Right, prediction.Rectangle.Top),
        new PointF(prediction.Rectangle.Right, prediction.Rectangle.Bottom),
        new PointF(prediction.Rectangle.Left, prediction.Rectangle.Bottom)
    ));

    image.Mutate<Rgba32>(a => a.DrawText($"{prediction.Label.Name} ({score})",
        font, prediction.Label.Color, new PointF(x, y)));
}

await image.SaveAsync("Assets/result.jpg");