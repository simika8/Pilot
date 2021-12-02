# get 
```https://localhost:44339/odata/DemoProduct?$top=10&$filter=startswith(tolower(Name),'aaaa')```


# post 
```https://localhost:44339/odata/DemoProduct```

body:
```json
{
    "Id": "00000000-0000-0000-0000-000000000001",
    "Name": "Aaaa1 proba",
    "Active": true,
    "Price": 123.1,
    "ReleaseDate": "2021-12-01T02:00:00.03+01:00",
    "Rating": 2,
    "Type": "Product"
}
``` 


# put 
```https://localhost:44339/odata/DemoProduct(00000000-0000-0000-0000-000000000001)```

body:
```json
{
    "Id": "00000000-0000-0000-0000-000000000001",
    "Name": "Aaaa1 proba",
    "Active": true,
    "ReleaseDate": "2021-12-01T02:00:00.03+01:00",
    "Rating": 2,
    "Type": "Product"
}
```

# patch 
```https://localhost:44339/odata/DemoProduct(00000000-0000-0000-0000-000000000001)```

body:
```json
{
    "Name": "Aaaa1 proba 123",
}
```

# delete 
```https://localhost:44339/odata/DemoProduct(00000000-0000-0000-0000-000000000001)```

