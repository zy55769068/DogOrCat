// 2023-11-27 18:00

let header = $request.headers;

header["MConfig-Info"] = '你的数据';
header["User-Agent"] = "你的数据";
header["Cookie"] = '你的数据';

$done({ headers: header });
