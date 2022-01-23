INSERT INTO t_ticket (
	ticket_id
,	title
,	status
)
VALUES(
	1
, 	"타이틀1"
, 	"In Progress"
);

SELECT
	A.ticket_id
,	A.title
,	A.status
FROM
	t_ticket A
WHERE 
	A.status = @STATUS;
	
	
DELETE FROM t_ticket 