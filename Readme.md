TASK_Variant 22 (22%3 = 1)

Lot endpoints:

1) Lot info. 
ENDPOINT: GET /api/v1/lot/{id}
PARAMS: Id:GUID.
RETURNS: Id:GUID, AuctionId:GUID, Title:string, Description:string(optional), StartPrice:decimal, PriceStep:decimal
POSIBLE STATUS CODES: 200, 404

2) List of auction lots. 
ENDPOINT: GET /api/v1/lot/auction/{auctionId}
PARAMS: auctionId:GUID.
RETURNS: Id:GUID, AuctionId:GUID, Title:string, Description:string(optional), StartPrice:decimal, PriceStep:decimal.
POSIBLE STATUS CODES: 200, 400, 404

3) Create lot for future auction. 
ENDPOINT: POST /api/v1/lot
PARAMS: AuctionId:GUID, Title:string, Description:string(optional), StartPrice:decimal, PriceStep:decimal.
RETURNS: Id:GUID
POSIBLE STATUS CODES: 200, 400

4) Modify lot in future auction. 
ENDPOINT: PUT /api/v1/lot/{id}
PARAMS: Id:GUID, Title:string, Description:string(optional), StartPrice:decimal, PriceStep:decimal.
RETURNS: None.
POSIBLE STATUS CODES: 204, 400, 404

5) Delete lot in future auction. 
ENDPOINT: DELETE /api/v1/lot/{id}
PARAMS: Id:GUID.
RETURNS: None.
POSIBLE STATUS CODES: 204, 400, 404

___
<h4>Aditional</h4>

1. RUN WITH DOTNET CLI RUN FROM

"auction_course\SothbeysKillerApi"

>dotnet run --launch-profile https


2. RUN DOCKER :

>docker compose up 

3. CREATE TABLES IN DB : 

1)Create table auctions

CREATE TABLE auctions(
    Id uuid NOT NULL PRIMARY KEY,
    Title varchar(255) NOT NULL,
    Start timestamp NOT NULL,
    Finish timestamp NOT NULL
);

2)Create table lots
DEcimal => numeric(18, 2)
