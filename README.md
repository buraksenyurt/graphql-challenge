# GraphQL Challenge

Uygulamanın senaryosu şöyle...Bir e-ticaret platformunun servislerinden birisini geliştiriyoruz. Arzu edilen fonksiyonelliklerden birisi, müşterinin baktığı ürüne ait bilgiler haricinde, kullanıcı yorumlarını ve görsellerini de döndürebilmek. Bu veri setini istemci uygulamanın (kuvvetle muhtemel önyüz tarafı) istediği şekilde sorgulayabilmesini istiyoruz. Yani birden fazla resource'u esnek bir şekilde sorgulayabileceğimiz imkanlarımız olsun derdindeyiz. Ürün bilgisinin içeren ana parça Entity Framework odaklı bir veri kaynağı olabilir. Kuvvetle muhtemel Postgresql iyi bir çözüm gibi duruyor. Diğer yandan yorumlar başka bir servisten gelecek. Ayrıca ürün görselleri de fiziki diskten okuma yapan (CDN gibi bölge de olabilir) bir enstrüman üstünden geliyor.

## Ön Hazırlıklar

```bash
# Postgresql tarafı için docker imajı kullanılabilir
docker run --name postgresql POSTGRES_DB=southhwind POSTGRES_USER=scoth -e POSTGRES_PASSWORD=tiger -p 5432:5432 -v /data:/var/lib/postgresql/data -d postgres

```
