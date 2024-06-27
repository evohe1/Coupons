


using amazongrpcclient;
using Grpc.Net.Client;
using GrpcService.Protos;

var httpHandler = new HttpClientHandler();
// Return `true` to allow certificates that are untrusted/invalid
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

//var handler = new SubdirectoryHandler(httpHandler, "/Amazon");
//var channel = GrpcChannel.ForAddress("https://grpc.kampanyan.com", new GrpcChannelOptions { HttpHandler = handler });

var channel = GrpcChannel.ForAddress("http://localhost:5006", new GrpcChannelOptions { HttpHandler = httpHandler });
//var channel = GrpcChannel.ForAddress("https://amazon.grpc.kampanyan.com", new GrpcChannelOptions { HttpHandler = httpHandler });

var crawlClient = new GrpcService.Protos.Crawl.CrawlClient(channel);

var link1 = "https://www.amazon.com.tr/Koton-Erkek-Dinozor-Bask%C4%B1l%C4%B1-Pamuklu/dp/B0B21Q9KH6/ref=sr_1_16?m=A1UNQM1SR2CHM&qid=1661962190&refinements=p_6%3AA1UNQM1SR2CHM%2Cp_36%3A-10000%2Cp_n_deal_type%3A26902948031&s=apparel&sr=1-16&srs=26249353031&th=1&psc=1";
var link = "https://www.amazon.com.tr/Arzum-Okka-Minio-Kahvesi-Makinesi/dp/B07QP7T8ZK/ref=lp_28163760031_1_2?th=1";
var link2 = "https://www.amazon.com.tr/JBL-CLIP-Bluetooth-hoparl%C3%B6r-siyah/dp/B08HRWSYH6/?_encoding=UTF8&pd_rd_w=Vv1lg&pf_rd_p=9afbb808-da8f-4056-bd9e-a8e751d05961&pf_rd_r=H9EHY5QXKEYAYM94Y5E7&pd_rd_r=9f274b63-46e7-410d-a4af-f95600b4fa12&pd_rd_wg=b1u5H&ref_=pd_gw_crs_zg_bs_12466497031";
var link3 = "https://www.amazon.com.tr/Philips-Hue-HDMI-Sync-Box/dp/B086MR9XR8/ref=pd_sim_2_sccl_5/257-5157582-0700100?pd_rd_w=sxyvT&pf_rd_p=1efcf801-149a-4b0e-8573-7ef48f28b427&pf_rd_r=Y2SNDKJS05AFC32PKKMX&pd_rd_r=bd8e1722-c921-4f62-8b5f-a1bf73ba6a51&pd_rd_wg=pzGUm&pd_rd_i=B086MR9XR8&psc=1";





ProductInfo deneme =await crawlClient.GetProductInfoAsync(new ProductUrl
{
    Url = link1
});
foreach (var item in deneme.Description)
{
    Console.WriteLine(item.Info);
   
}

foreach (var item in deneme.Images)
{
    Console.WriteLine(item);
}
foreach (var item in deneme.Category)
{
    Console.WriteLine(item);
}
Console.WriteLine(deneme.Price);
Console.WriteLine(deneme.PreviousPrice);