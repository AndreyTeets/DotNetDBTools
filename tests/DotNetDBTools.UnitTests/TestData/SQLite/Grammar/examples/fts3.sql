-- ===fts3aa.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1(content) VALUES('one');
INSERT INTO t1(content) VALUES('two');
INSERT INTO t1(content) VALUES('one two');
INSERT INTO t1(content) VALUES('three');
INSERT INTO t1(content) VALUES('one three');
INSERT INTO t1(content) VALUES('two three');
INSERT INTO t1(content) VALUES('one two three');
INSERT INTO t1(content) VALUES('four');
INSERT INTO t1(content) VALUES('one four');
INSERT INTO t1(content) VALUES('two four');
INSERT INTO t1(content) VALUES('one two four');
INSERT INTO t1(content) VALUES('three four');
INSERT INTO t1(content) VALUES('one three four');
INSERT INTO t1(content) VALUES('two three four');
INSERT INTO t1(content) VALUES('one two three four');
INSERT INTO t1(content) VALUES('five');
INSERT INTO t1(content) VALUES('one five');
INSERT INTO t1(content) VALUES('two five');
INSERT INTO t1(content) VALUES('one two five');
INSERT INTO t1(content) VALUES('three five');
INSERT INTO t1(content) VALUES('one three five');
INSERT INTO t1(content) VALUES('two three five');
INSERT INTO t1(content) VALUES('one two three five');
INSERT INTO t1(content) VALUES('four five');
INSERT INTO t1(content) VALUES('one four five');
INSERT INTO t1(content) VALUES('two four five');
INSERT INTO t1(content) VALUES('one two four five');
INSERT INTO t1(content) VALUES('three four five');
INSERT INTO t1(content) VALUES('one three four five');
INSERT INTO t1(content) VALUES('two three four five');
INSERT INTO t1(content) VALUES('one two three four five');

SELECT rowid FROM t1 WHERE content MATCH 'three two one';

SELECT rowid FROM t1 WHERE content MATCH 'one two THREE';

SELECT rowid FROM t1 WHERE content MATCH '  ONE    Two   three  ';

SELECT rowid FROM t1 WHERE content MATCH '"one"';

SELECT rowid FROM t1 WHERE content MATCH '"one two"';

SELECT rowid FROM t1 WHERE content MATCH '"two one"';

SELECT rowid FROM t1 WHERE content MATCH '"one two three"';

SELECT rowid FROM t1 WHERE content MATCH '"one three two"';

SELECT rowid FROM t1 WHERE content MATCH '"one two three four"';

SELECT rowid FROM t1 WHERE content MATCH '"one three two four"';

SELECT rowid FROM t1 WHERE content MATCH 'one';

SELECT rowid FROM t1 WHERE content MATCH '"one three five"';

SELECT rowid FROM t1 WHERE content MATCH '"one three" five';

SELECT rowid FROM t1 WHERE content MATCH 'five "one three"';

SELECT rowid FROM t1 WHERE content MATCH 'five "one three" four';

SELECT rowid FROM t1 WHERE content MATCH 'five four "one three"';

SELECT rowid FROM t1 WHERE content MATCH '"one three" four five';

SELECT rowid FROM t1 WHERE content MATCH 'one';

SELECT rowid FROM t1 WHERE content MATCH 'one -two';

SELECT rowid FROM t1 WHERE content MATCH '-two one';

SELECT rowid FROM t1 WHERE content MATCH 'one OR two';

SELECT rowid FROM t1 WHERE content MATCH 'one two';

SELECT rowid FROM t1 WHERE content MATCH '"one two" OR three';

SELECT rowid FROM t1 WHERE content MATCH 'three OR "one two"';

SELECT rowid FROM t1 WHERE content MATCH 'one two OR three';

SELECT rowid FROM t1 WHERE content MATCH 'three OR two one';

SELECT rowid FROM t1 WHERE content MATCH 'one two OR three OR four';

SELECT rowid FROM t1 WHERE content MATCH 'two OR three OR four one';

INSERT INTO t1(content) VALUES(NULL);

SELECT content FROM t1 WHERE rowid=rowid;

SELECT rowid FROM t1 WHERE content MATCH NULL;

INSERT INTO t1(rowid, content) VALUES(0, 'four five');

SELECT rowid FROM t1 WHERE content MATCH 'two one';

SELECT content FROM t1 WHERE rowid = 0;

INSERT INTO t1(rowid, content) VALUES(-1, 'three four');

SELECT content FROM t1 WHERE rowid = -1;

SELECT rowid FROM t1 WHERE t1 MATCH 'four';

SELECT rowid FROM t1 WHERE content MATCH 'one two three';

SELECT rowid FROM t1 WHERE content MATCH 'one three two';

SELECT rowid FROM t1 WHERE content MATCH 'two three one';

SELECT rowid FROM t1 WHERE content MATCH 'two one three';

SELECT rowid FROM t1 WHERE content MATCH 'three one two';

-- ===fts3ab.test===
CREATE VIRTUAL TABLE t1 USING fts3(english,spanish,german);

CREATE VIRTUAL TABLE t4 USING fts3([norm],'plusone',"invert");

SELECT rowid FROM t4 WHERE t4 MATCH 'norm:one';

SELECT rowid FROM t4 WHERE norm MATCH 'one';

SELECT rowid FROM t4 WHERE t4 MATCH 'one';

SELECT rowid FROM t4 WHERE t4 MATCH 'plusone:one';

SELECT rowid FROM t4 WHERE plusone MATCH 'one';

SELECT rowid FROM t4 WHERE t4 MATCH 'norm:one plusone:two';

SELECT rowid FROM t4 WHERE t4 MATCH 'norm:one two';

SELECT rowid FROM t4 WHERE t4 MATCH 'plusone:two norm:one';

SELECT rowid FROM t4 WHERE t4 MATCH 'two norm:one';

SELECT rowid FROM t1 WHERE english MATCH 'one';

SELECT rowid FROM t1 WHERE spanish MATCH 'one';

SELECT rowid FROM t1 WHERE german MATCH 'one';

SELECT rowid FROM t1 WHERE t1 MATCH 'one';

SELECT rowid FROM t1 WHERE t1 MATCH 'one dos drei';

SELECT english, spanish, german FROM t1 WHERE rowid=1;

SELECT rowid FROM t1 WHERE t1 MATCH '"one un"';

CREATE VIRTUAL TABLE t2 USING fts3(from,to);
INSERT INTO t2([from],[to]) VALUES ('one two three', 'four five six');
SELECT [from], [to] FROM t2;

-- ===fts3ac.test===
CREATE VIRTUAL TABLE email USING fts3([from],[to],subject,body);
BEGIN TRANSACTION;
INSERT INTO email([from],[to],subject,body) VALUES('savita.puthigai@enron.com', 'traders.eol@enron.com, traders.eol@enron.com', 'EnronOnline- Change to Autohedge', 'Effective Monday, October 22, 2001 the following changes will be made to the Autohedge functionality on EnronOnline.
The volume on the hedge will now respect the minimum volume and volume increment settings on the parent product. See rules below: 
?	If the transaction volume on the child is less than half of the parent''s minimum volume no hedge will occur.
?	If the transaction volume on the child is more than half the parent''s minimum volume but less than half the volume increment on the parent, the hedge will volume will be the parent''s minimum volume.
?	For all other volumes, the same rounding rules will apply based on the volume increment on the parent product.
Please see example below:
Parent''s Settings:
Minimum: 	5000
Increment:  1000
Volume on Autohedge transaction			Volume Hedged
1      - 2499							0
2500 - 5499							5000
5500 - 6499							6000');
INSERT INTO email([from],[to],subject,body) VALUES('dana.davis@enron.com', 'laynie.east@enron.com, lisa.king@enron.com, lisa.best@enron.com,', 'Leaving Early', 'FYI:  
If it''s ok with everyone''s needs, I would like to leave @4pm. If you think 
you will need my assistance past the 4 o''clock hour just let me know;  I''ll 
be more than willing to stay.');
INSERT INTO email([from],[to],subject,body) VALUES('enron_update@concureworkplace.com', 'louise.kitchen@enron.com', '<<Concur Expense Document>> - CC02.06.02', 'The following expense report is ready for approval:
Employee Name: Christopher F. Calger
Status last changed by: Mollie E. Gustafson Ms
Expense Report Name: CC02.06.02
Report Total: 3,972.93
Amount Due Employee: 3,972.93
To approve this expense report, click on the following link for Concur Expense.
http://expensexms.enron.com');
INSERT INTO email([from],[to],subject,body) VALUES('jeff.duff@enron.com', 'julie.johnson@enron.com', 'Work request', 'Julie,
Could you print off the current work request report by 1:30 today?
Gentlemen,
I''d like to review this today at 1:30 in our office.  Also, could you provide 
me with your activity reports so I can have Julie enter this information.
JD');
INSERT INTO email([from],[to],subject,body) VALUES('v.weldon@enron.com', 'gary.l.carrier@usa.dupont.com, scott.joyce@bankofamerica.com', 'Enron News', 'This could turn into something big.... 
http://biz.yahoo.com/rf/010129/n29305829.html');
INSERT INTO email([from],[to],subject,body) VALUES('mark.haedicke@enron.com', 'paul.simons@enron.com', 'Re: First Polish Deal!', 'Congrats!  Things seem to be building rapidly now on the Continent.  Mark');
INSERT INTO email([from],[to],subject,body) VALUES('e..carter@enron.com', 't..robinson@enron.com', 'FW: Producers Newsletter 9-24-2001', '
The producer lumber pricing sheet.
From: 	Johnson, Jay  
Sent:	Tuesday, October 16, 2001 3:42 PM
To:	Carter, Karen E.
Subject:	FW: Producers Newsletter 9-24-2001
From: 	Daigre, Sergai  
Sent:	Friday, September 21, 2001 8:33 PM
Subject:	Producers Newsletter 9-24-2001
');
INSERT INTO email([from],[to],subject,body) VALUES('david.delainey@enron.com', 'kenneth.lay@enron.com', 'Greater Houston Partnership', 'Ken, in response to the letter from Mr Miguel San Juan, my suggestion would 
be to offer up the Falcon for their use; however, given the tight time frame 
and your recent visit with Mr. Fox that it would be difficult for either you 
or me to participate.
I spoke to Max and he agrees with this approach.
I hope this meets with your approval.
Regards
Delainey');
INSERT INTO email([from],[to],subject,body) VALUES('lachandra.fenceroy@enron.com', 'lindy.donoho@enron.com', 'FW: Bus Applications Meeting Follow Up', 'Lindy,
Here is the original memo we discussed earlier.  Please provide any information that you may have.
Your cooperation is greatly appreciated.
Thanks,
lachandra.fenceroy@enron.com
713.853.3884
877.498.3401 Pager
From: 	Bisbee, Joanne  
Sent:	Wednesday, September 26, 2001 7:50 AM
To:	Fenceroy, LaChandra
Subject:	FW: Bus Applications Meeting Follow Up
Lachandra, Please get with David Duff today and see what this is about.  Who are our TW accounting business users?
From: 	Koh, Wendy  
Sent:	Tuesday, September 25, 2001 2:41 PM
To:	Bisbee, Joanne
Subject:	Bus Applications Meeting Follow Up
Lisa brought up a TW change effective Nov 1.  It involves eliminating a turnback surcharge.  I have no other information, but you might check with the business folks for any system changes required.
Wendy');
INSERT INTO email([from],[to],subject,body) VALUES('danny.mccarty@enron.com', 'fran.fagan@enron.com', 'RE: worksheets', 'Fran,
If Julie''s merit needs to be lump sum, just move it over to that column.  Also, send me Eric Gadd''s sheets as well.  Thanks.
Dan
From: 	Fagan, Fran  
Sent:	Thursday, December 20, 2001 11:10 AM
To:	McCarty, Danny
Subject:	worksheets
As discussed, attached are your sheets for bonus and merit.
Thanks,
Fran Fagan
Sr. HR Rep
713.853.5219
<< File: McCartyMerit.xls >>  << File: mccartyBonusCommercial_UnP.xls >> 
');
INSERT INTO email([from],[to],subject,body) VALUES('bert.meyers@enron.com', 'shift.dl-portland@enron.com', 'OCTOBER SCHEDULE', 'TEAM,
PLEASE SEND ME ANY REQUESTS THAT YOU HAVE FOR OCTOBER.  SO FAR I HAVE THEM FOR LEAF.  I WOULD LIKE TO HAVE IT DONE BY THE 15TH OF THE MONTH.  ANY QUESTIONS PLEASE GIVE ME A CALL.
BERT');
INSERT INTO email([from],[to],subject,body) VALUES('errol.mclaughlin@enron.com', 'john.arnold@enron.com, bilal.bajwa@enron.com, john.griffith@enron.com,', 'TRV Notification:  (NG - PROPT P/L - 09/27/2001)', 'The report named: NG - PROPT P/L <http://trv.corp.enron.com/linkFromExcel.asp?report_cd=11&report_name=NG+-+PROPT+P/L&category_cd=5&category_name=FINANCIAL&toc_hide=1&sTV1=5&TV1Exp=Y&current_efct_date=09/27/2001>, published as of 09/27/2001 is now available for viewing on the website.');
INSERT INTO email([from],[to],subject,body) VALUES('patrice.mims@enron.com', 'calvin.eakins@enron.com', 'Re: Small business supply assistance', 'Hi Calvin
I spoke with Rickey (boy, is he long-winded!!).  Gave him the name of our 
credit guy, Russell Diamond.
Thank for your help!');
INSERT INTO email([from],[to],subject,body) VALUES('legal <.hall@enron.com>', 'stephanie.panus@enron.com', 'Termination update', 'City of Vernon and Salt River Project terminated their contracts.  I will fax these notices to you.');
INSERT INTO email([from],[to],subject,body) VALUES('d..steffes@enron.com', 'richard.shapiro@enron.com', 'EES / ENA Government Affairs Staffing & Outside Services', 'Rick Here is the information on staffing and outside services.  Call if you need anything else.
Jim
');
INSERT INTO email([from],[to],subject,body) VALUES('gelliott@industrialinfo.com', 'pcopello@industrialinfo.com', 'ECAAR (Gavin), WSCC (Diablo Canyon), & NPCC (Seabrook)', 'Dear Power Outage Database Customer, 
Attached you will find an excel document. The outages contained within are forced or rescheduled outages. Your daily delivery will still contain these outages. 
In addition to the two excel documents, there is a dbf file that is formatted like your daily deliveries you receive nightly. This will enable you to load the data into your regular database. Any questions please let me know. Thanks. 
Greg Elliott 
IIR, Inc. 
713-783-5147 x 3481 
outages@industrialinfo.com 
THE INFORMATION CONTAINED IN THIS E-MAIL IS LEGALLY PRIVILEGED AND CONFIDENTIAL INFORMATION INTENDED ONLY FOR THE USE OF THE INDIVIDUAL OR ENTITY NAMED ABOVE.  YOU ARE HEREBY NOTIFIED THAT ANY DISSEMINATION, DISTRIBUTION, OR COPY OF THIS E-MAIL TO UNAUTHORIZED ENTITIES IS STRICTLY PROHIBITED. IF YOU HAVE RECEIVED THIS 
E-MAIL IN ERROR, PLEASE DELETE IT.
- OUTAGE.dbf 
- 111201R.xls 
- 111201.xls ');
INSERT INTO email([from],[to],subject,body) VALUES('enron.announcements@enron.com', 'all_ena_egm_eim@enron.com', 'EWS Brown Bag', 'MARK YOUR LUNCH CALENDARS NOW !
You are invited to attend the EWS Brown Bag Lunch Series
Featuring:   RAY BOWEN, COO
Topic:  Enron Industrial Markets
Thursday, March 15, 2001
11:30 am - 12:30 pm
EB 5 C2
You bring your lunch,           Limited Seating
We provide drinks and dessert.          RSVP  x 3-9610');
INSERT INTO email([from],[to],subject,body) VALUES('chris.germany@enron.com', 'ingrid.immer@williams.com', 'Re: About St Pauls', 'Sounds good to me.  I bet this is next to the Warick?? Hotel.
"Immer, Ingrid" <Ingrid.Immer@Williams.com> on 12/21/2000 11:48:47 AM
To: "''chris.germany@enron.com''" <chris.germany@enron.com>
cc:  
Subject: About St Pauls
<<About St Pauls.url>>  
? 
?http://www.stpaulshouston.org/about.html 
Chris, 
I like the looks of this place.? What do you think about going here Christmas 
eve?? They have an 11:00 a.m. service and a candlelight service at 5:00 p.m., 
among others.
Let me know.?? ii 
- About St Pauls.url
');
INSERT INTO email([from],[to],subject,body) VALUES('nas@cpuc.ca.gov', 'skatz@sempratrading.com, kmccrea@sablaw.com, thompson@wrightlaw.com,', 'Reply Brief filed July 31, 2000', ' - CPUC01-#76371-v1-Revised_Reply_Brief__Due_today_7_31_.doc');
INSERT INTO email([from],[to],subject,body) VALUES('gascontrol@aglresources.com', 'dscott4@enron.com, lcampbel@enron.com', 'Alert Posted 10:00 AM November 20,2000: E-GAS Request Reminder', 'Alert Posted 10:00 AM November 20,2000: E-GAS Request Reminder
As discussed in the Winter Operations Meeting on Sept.29,2000, 
E-Gas(Emergency Gas) will not be offered this winter as a service from AGLC.  
Marketers and Poolers can receive gas via Peaking and IBSS nominations(daisy 
chain) from other marketers up to the 6 p.m. Same Day 2 nomination cycle.
');
INSERT INTO email([from],[to],subject,body) VALUES('dutch.quigley@enron.com', 'rwolkwitz@powermerchants.com', '', ' 
Here is a goody for you');
INSERT INTO email([from],[to],subject,body) VALUES('ryan.o''rourke@enron.com', 'k..allen@enron.com, randy.bhatia@enron.com, frank.ermis@enron.com,', 'TRV Notification:  (West VaR - 11/07/2001)', 'The report named: West VaR <http://trv.corp.enron.com/linkFromExcel.asp?report_cd=36&report_name=West+VaR&category_cd=2&category_name=WEST&toc_hide=1&sTV1=2&TV1Exp=Y&current_efct_date=11/07/2001>, published as of 11/07/2001 is now available for viewing on the website.');
INSERT INTO email([from],[to],subject,body) VALUES('mjones7@txu.com', 'cstone1@txu.com, ggreen2@txu.com, timpowell@txu.com,', 'Enron / HPL Actuals for July 10, 2000', 'Teco Tap       10.000 / Enron ; 110.000 / HPL IFERC
LS HPL LSK IC       30.000 / Enron
');
INSERT INTO email([from],[to],subject,body) VALUES('susan.pereira@enron.com', 'kkw816@aol.com', 'soccer practice', 'Kathy-
Is it safe to assume that practice is cancelled for tonight??
Susan Pereira');
INSERT INTO email([from],[to],subject,body) VALUES('mark.whitt@enron.com', 'barry.tycholiz@enron.com', 'Huber Internal Memo', 'Please look at this.  I didn''t know how deep to go with the desk.  Do you think this works.
');
INSERT INTO email([from],[to],subject,body) VALUES('m..forney@enron.com', 'george.phillips@enron.com', '', 'George,
Give me a call and we will further discuss opportunities on the 13st floor.
Thanks,
JMForney
3-7160');
INSERT INTO email([from],[to],subject,body) VALUES('brad.mckay@enron.com', 'angusmcka@aol.com', 'Re: (no subject)', 'not yet');
INSERT INTO email([from],[to],subject,body) VALUES('adam.bayer@enron.com', 'jonathan.mckay@enron.com', 'FW: Curve Fetch File', 'Here is the curve fetch file sent to me.  It has plenty of points in it.  If you give me a list of which ones you need we may be able to construct a secondary worksheet to vlookup the values.
adam
35227
From: 	Royed, Jeff  
Sent:	Tuesday, September 25, 2001 11:37 AM
To:	Bayer, Adam
Subject:	Curve Fetch File
Let me know if it works.   It may be required to have a certain version of Oracle for it to work properly.
Jeff Royed
Enron 
Energy Operations
Phone: 713-853-5295');
INSERT INTO email([from],[to],subject,body) VALUES('matt.smith@enron.com', 'yan.wang@enron.com', 'Report Formats', 'Yan,
The merged reports look great.  I believe the only orientation changes are to 
"unmerge" the following six reports:  
31 Keystone Receipts
15 Questar Pipeline
40 Rockies Production
22 West_2
23 West_3
25 CIG_WIC
The orientation of the individual reports should be correct.  Thanks.
Mat
PS.  Just a reminder to add the "*" by the title of calculated points.');
INSERT INTO email([from],[to],subject,body) VALUES('michelle.lokay@enron.com', 'jimboman@bigfoot.com', 'Egyptian Festival', '10:08 AM "Karkour, Randa" <Randa.Karkour@COMPAQ.com> on 09/07/2000 09:01:04 AM
To: "''Agheb (E-mail)" <Agheb@aol.com>, "Leila Mankarious (E-mail)" 
<Leila_Mankarious@mhhs.org>, "''Marymankarious (E-mail)" 
<marymankarious@aol.com>, "Michelle lokay (E-mail)" <mlokay@enron.com>, "Ramy 
Mankarious (E-mail)" <Mankarious@aol.com>
cc:  
Subject: Egyptian Festival
<<Egyptian Festival.url>>
http://www.egyptianfestival.com/
- Egyptian Festival.url
');
INSERT INTO email([from],[to],subject,body) VALUES('errol.mclaughlin@enron.com', 'sherry.dawson@enron.com', 'Urgent!!! Thanks
08:39 AM From:  William Kelly @ ECT                           12/20/2000 08:31 AM
To: Kam Keiser/HOU/ECT@ECT, Darron C Giron/HOU/ECT@ECT, David 
Baumbach/HOU/ECT@ECT, Errol McLaughlin/Corp/Enron@ENRON
cc: Kimat Singla/HOU/ECT@ECT, Kulvinder Fowler/NA/Enron@ENRON, Kyle R 
Lilly/HOU/ECT@ECT, Jeff Royed/Corp/Enron@ENRON, Alejandra 
Chavez/NA/Enron@ENRON, Crystal Hyde/HOU/ECT@ECT 
Subject: New EAST books
We have new book names in TAGG for our intramonth portfolios and it is 
extremely important that any deal booked to the East is communicated quickly 
to someone on my team.  I know it will take some time for the new names to 
sink in and I do not want us to miss any positions or P&L.  
Thanks for your help on this.
New:
Scott Neal :         East Northeast
Dick Jenkins:     East Marketeast
WK 
');
INSERT INTO email([from],[to],subject,body) VALUES('david.forster@enron.com', 'eol.wide@enron.com', 'Change to Stack Manager', 'Effective immediately, there is a change to the Stack Manager which will 
affect any Inactive Child.
An inactive Child with links to Parent products will not have their 
calculated prices updated until the Child product is Activated.
When the Child Product is activated, the price will be recalculated and 
updated BEFORE it is displayed on the web.
This means that if you are inputting a basis price on a Child product, you 
will not see the final, calculated price until you Activate the product, at 
which time the customer will also see it.
If you have any questions, please contact the Help Desk on:
Americas: 713 853 4357
Europe: + 44 (0) 20 7783 7783
Asia/Australia: +61 2 9229 2300
Dave');
INSERT INTO email([from],[to],subject,body) VALUES('vince.kaminski@enron.com', 'jhh1@email.msn.com', 'Re: Light reading - see pieces beginning on page 7', 'John,
I saw it. Very interesting.
Vince
"John H Herbert" <jhh1@email.msn.com> on 07/28/2000 08:38:08 AM
To: "Vince J Kaminski" <Vince_J_Kaminski@enron.com>
cc:  
Subject: Light reading - see pieces beginning on page 7
Cheers and have a nice weekend,
JHHerbert
- gd000728.pdf
');
INSERT INTO email([from],[to],subject,body) VALUES('matthew.lenhart@enron.com', 'mmmarcantel@equiva.com', 'RE:', 'i will try to line up a pig for you ');
INSERT INTO email([from],[to],subject,body) VALUES('jae.black@enron.com', 'claudette.harvey@enron.com, chaun.roberts@enron.com, judy.martinez@enron.com,', 'Disaster Recovery Equipment', 'As a reminder...there are several pieces of equipment that are set up on the 30th Floor, as well as on our floor, for the Disaster Recovery Team.  PLEASE DO NOT TAKE, BORROW OR USE this equipment.  Should you need to use another computer system, other than yours, or make conference calls please work with your Assistant to help find or set up equipment for you to use.
Thanks for your understanding in this matter.
T.Jae Black
East Power Trading
Assistant to Kevin Presto
off. 713-853-5800
fax 713-646-8272
cell 713-539-4760');
INSERT INTO email([from],[to],subject,body) VALUES('eric.bass@enron.com', 'dale.neuner@enron.com', '5 X 24', 'Dale,
Have you heard anything more on the 5 X 24s?  We would like to get this 
product out ASAP.
Thanks,
Eric');
INSERT INTO email([from],[to],subject,body) VALUES('messenger@smartreminders.com', 'm..tholt@enron.com', '10% Coupon - PrintPal Printer Cartridges - 100% Guaranteed', '[IMAGE]
[IMAGE][IMAGE][IMAGE] 
Dear  SmartReminders Member,
[IMAGE]         [IMAGE]        [IMAGE]     [IMAGE]    [IMAGE]    [IMAGE]        [IMAGE]      [IMAGE]     	
We respect  your privacy and are a Certified Participant of the BBBOnLine
Privacy Program.  To be removed from future offers,click  here. 
SmartReminders.com  is a permission based service. To unsubscribe click  here .  ');
INSERT INTO email([from],[to],subject,body) VALUES('benjamin.rogers@enron.com', 'mark.bernstein@enron.com', '', 'The guy you are talking about left CIN under a "cloud of suspicion" sort of 
speak.  He was the one who got into several bad deals and PPA''s in California 
for CIN, thus he left on a bad note.  Let me know if you need more detail 
than that, I felt this was the type of info you were looking for.  Thanks!
Ben');
INSERT INTO email([from],[to],subject,body) VALUES('enron_update@concureworkplace.com', 'michelle.cash@enron.com', 'Expense Report Receipts Not Received', 'Employee Name: Michelle Cash
Report Name:   Houston Cellular 8-11-01
Report Date:   12/13/01
Report ID:     594D37C9ED2111D5B452
Submitted On:  12/13/01
You are only allowed 2 reports with receipts outstanding.  Your expense reports will not be paid until you meet this requirement.');
INSERT INTO email([from],[to],subject,body) VALUES('susan.mara@enron.com', 'ray.alvarez@enron.com, mark.palmer@enron.com, karen.denne@enron.com,', 'CAISO Emergency Motion Sue Mara
Enron Corp.
Tel: (415) 782-7802
Fax:(415) 782-7854
"Milner, Marcie" <MMilner@coral-energy.com> 06/08/2001 11:13 AM 	   To: "''smara@enron.com''" <smara@enron.com>  cc:   Subject: CAISO Emergency Motion	
Sue, did you see this emergency motion the CAISO filed today?  Apparently
they are requesting that FERC discontinue market-based rates immediately and
grant refunds plus interest on the difference between cost-based rates and
market revenues received back to May 2000.  They are requesting the
commission act within 14 days.  Have you heard anything about what they are
doing?
Marcie
http://www.caiso.com/docs/2001/06/08/200106081005526469.pdf 
');
INSERT INTO email([from],[to],subject,body) VALUES('fletcher.sturm@enron.com', 'eloy.escobar@enron.com', 'Re: General Brinks Position Meeting', 'Eloy,
Who is General Brinks?
Fletch');
INSERT INTO email([from],[to],subject,body) VALUES('nailia.dindarova@enron.com', 'richard.shapiro@enron.com', 'Documents for Mark Frevert (on EU developments and lessons from', 'Rick,
Here are the documents that Peter has prepared for Mark Frevert. 
Nailia
16:36 Nailia Dindarova
25/06/2001 15:36
To: Michael Brown/Enron@EUEnronXGate
cc: Ross Sankey/Enron@EUEnronXGate, Eric Shaw/ENRON@EUEnronXGate, Peter 
Styles/LON/ECT@ECT 
Subject: Documents for Mark Frevert (on EU developments and lessons from 
California)
Michael,
These are the documents that Peter promised to give to you for Mark Frevert. 
He has now handed them to him in person but asked me to transmit them 
electronically to you, as well as Eric and Ross.
Nailia
');
INSERT INTO email([from],[to],subject,body) VALUES('peggy.a.kostial@accenture.com', 'dave.samuels@enron.com', 'EOL-Accenture Deal Sheet', 'Dave -
Attached are our comments and suggested changes. Please call to review.
On the time line for completion, we have four critical steps to complete:
Finalize market analysis to refine business case, specifically
projected revenue stream
Complete counterparty surveying, including targeting 3 CPs for letters
of intent
Review Enron asset base for potential reuse/ licensing
Contract negotiations
Joe will come back to us with an updated time line, but it is my
expectation that we are still on the same schedule (we just begun week
three) with possibly a week or so slippage.....contract negotiations will
probably be the critical path.
We will send our cut at the actual time line here shortly. Thanks,
Peggy
(See attached file: accenture-dealpoints v2.doc)
- accenture-dealpoints v2.doc ');
INSERT INTO email([from],[to],subject,body) VALUES('thomas.martin@enron.com', 'thomas.martin@enron.com', 'Re: Guadalupe Power Partners LP', '03:49 PM Thomas A Martin
10/11/2000 03:55 PM
To: Patrick Wade/HOU/ECT@ECT
cc:  
Subject: Re: Guadalupe Power Partners LP  
The deal is physically served at Oasis Waha or Oasis Katy and is priced at 
either HSC, Waha or Katytailgate GD at buyers option three days prior to 
NYMEX  close.
');
INSERT INTO email([from],[to],subject,body) VALUES('judy.townsend@enron.com', 'dan.junek@enron.com, chris.germany@enron.com', 'Columbia Distribution''s Capacity Available for Release - Sum', 'AM agoddard@nisource.com on 03/08/2001 09:16:57 AM
To: "        -         *Koch, Kent" <kkoch@nisource.com>, "        -         
*Millar, Debra" <dmillar@nisource.com>, "        -         *Burke, Lynn" 
<lburke@nisource.com>
cc: "        -         *Heckathorn, Tom" <theckathorn@nisource.com> 
Subject: Columbia Distribution''s Capacity Available for Release - Sum
Attached is Columbia Distribution''s notice of capacity available for release
for
the summer of 2001 (Apr. 2001 through Oct. 2001).
Please note that the deadline for bids is 3:00pm EST on March 20, 2001.
If you have any questions, feel free to contact any of the representatives
listed
at the bottom of the attachment.
Aaron Goddard
- 2001Summer.doc
');
INSERT INTO email([from],[to],subject,body) VALUES('rhonda.denton@enron.com', 'tim.belden@enron.com, dana.davis@enron.com, genia.fitzgerald@enron.com,', 'Split Rock Energy LLC', 'We have received the executed EEI contract from this CP dated 12/12/2000.  
Copies will be distributed to Legal and Credit.');
INSERT INTO email([from],[to],subject,body) VALUES('kerrymcelroy@dwt.com', 'jack.speer@alcoa.com, crow@millernash.com, michaelearly@earthlink.net,', 'Oral Argument Request', ' - Oral Argument Request.doc');
INSERT INTO email([from],[to],subject,body) VALUES('mike.carson@enron.com', 'rlmichaelis@hormel.com', '', 'Did you come in town this wk end..... My new number at our house is : 
713-668-3712...... my cell # is 281-381-7332
the kid');
INSERT INTO email([from],[to],subject,body) VALUES('cooper.richey@enron.com', 'trycooper@hotmail.com', 'FW: Contact Info', '
From: Punja, Karim 
Sent: Thursday, December 13, 2001 2:35 PM
To: Richey, Cooper
Subject: Contact Info
Cooper,
Its been a real pleasure working with you (even though it was for only a small amount of time)
I hope we can stay in touch.
Home# 234-0249
email: kpunja@hotmail.com
Take Care, 
Karim.
');
INSERT INTO email([from],[to],subject,body) VALUES('bjm30@earthlink.net', 'mcguinn.k@enron.com, mcguinn.ian@enron.com, mcguinn.stephen@enron.com,', 'email address change', 'Hello all.
I haven''t talked to many of you via email recently but I do want to give you
my new address for your email file:
bjm30@earthlink.net
I hope all is well.
Brian McGuinn');
INSERT INTO email([from],[to],subject,body) VALUES('shelley.corman@enron.com', 'steve.hotte@enron.com', 'Flat Panels', 'Can you please advise what is going on with the flat panels that we had planned to distribute to our gas logistics team.  It was in the budget and we had the okay, but now I''m hearing there is some hold-up & the units are stored on 44.
Shelley');
INSERT INTO email([from],[to],subject,body) VALUES('sara.davidson@enron.com', 'john.schwartzenburg@enron.com, scott.dieball@enron.com, recipients@enron.com,', '2001 Enron Law Conference (Distribution List 2)', '    Enron Law Conference
San Antonio, Texas    May 2-4, 2001    Westin Riverwalk
See attached memo for more details!!
? Registration for the law conference this year will be handled through an 
Online RSVP Form on the Enron Law Conference Website at 
http://lawconference.corp.enron.com.  The website is still under construction 
and will not be available until Thursday, March 15, 2001.  
? We will send you another e-mail to confirm when the Law Conference Website 
is operational. 
? Please complete the Online RSVP Form as soon as it is available  and submit 
it no later than Friday, March 30th.  
');
INSERT INTO email([from],[to],subject,body) VALUES('tori.kuykendall@enron.com', 'heath.b.taylor@accenture.com', 'Re:', 'hey - thats funny about john - he definitely remembers him - i''ll call pat 
and let him know - we are coming on saturday - i just havent had a chance to 
call you guys back directions again though');
INSERT INTO email([from],[to],subject,body) VALUES('darron.giron@enron.com', 'bryce.baxter@enron.com', 'Re: Feedback for Audrey Cook', 'Bryce,
I''ll get it done today.  
DG    3-9573
From:  Bryce Baxter                           06/12/2000 07:15 PM
To: Darron C Giron/HOU/ECT@ECT
cc:  
Subject: Feedback for Audrey Cook
You were identified as a reviewer for Audrey Cook.  If possible, could you 
complete her feedback by end of business Wednesday?  It will really help me 
in the PRC process to have your input.  Thanks.
');
INSERT INTO email([from],[to],subject,body) VALUES('casey.evans@enron.com', 'stephanie.sever@enron.com', 'Gas EOL ID', 'Stephanie,
In conjunction with the recent movement of several power traders, they are changing the names of their gas books as well.  The names of the new gas books and traders are as follows:
PWR-NG-LT-SPP:  Mike Carson
PWR-NG-LT-SERC:  Jeff King
If you need to know their power desk to map their ID to their gas books, those desks are as follows:
EPMI-LT-SPP:  Mike Carson
EPMI-LT-SERC:  Jeff King
I will be in training this afternoon, but will be back when class is over.  Let me know if you have any questions.
Thanks for your help!
Casey');
INSERT INTO email([from],[to],subject,body) VALUES('darrell.schoolcraft@enron.com', 'david.roensch@enron.com, kimberly.watson@enron.com, michelle.lokay@enron.com,', 'Postings', 'Please see the attached.
ds
');
INSERT INTO email([from],[to],subject,body) VALUES('mcominsky@aol.com', 'cpatman@bracepatt.com, james_derrick@enron.com', 'Jurisprudence Luncheon', 'Carrin & Jim It was an honor and a pleasure to meet both of you yesterday.  I know we will
have fun working together on this very special event.
Jeff left the jurisprudence luncheon lists for me before he left on vacation.
I wasn''t sure whether he transmitted them to you as well.  Would you please
advise me if you would like them sent to you?  I can email the MS Excel files
or I can fax the hard copies to you.   Please advise what is most convenient.
I plan to be in town through the holidays and can be reached by phone, email,
or cell phone at any time.  My cell phone number is 713/705-4829.
Thanks again for your interest in the ADL''s work.  Martin.
Martin B. Cominsky
Director, Southwest Region
Anti-Defamation League
713/627-3490, ext. 122
713/627-2011 (fax)
MCominsky@aol.com');
INSERT INTO email([from],[to],subject,body) VALUES('phillip.love@enron.com', 'todagost@utmb.edu, gbsonnta@utmb.edu', 'New President', 'I had a little bird put a word in my ear.  Is there any possibility for Ben 
Raimer to be Bush''s secretary of HHS?  Just curious about that infamous UTMB 
rumor mill.  Hope things are well, happy holidays.
PL');
INSERT INTO email([from],[to],subject,body) VALUES('marie.heard@enron.com', 'ehamilton@fna.com', 'ISDA Master Agreement', 'Erin:
Pursuant to your request, attached are the Schedule to the ISDA Master Agreement, together with Paragraph 13 to the ISDA Credit Support Annex.  Please let me know if you need anything else.  We look forward to hearing your comments.
Marie
Marie Heard
Senior Legal Specialist
Enron North America Corp.
Phone:  (713) 853-3907
Fax:  (713) 646-3490
marie.heard@enron.com
');
INSERT INTO email([from],[to],subject,body) VALUES('andrea.ring@enron.com', 'beverly.beaty@enron.com', 'Re: Tennessee Buy - Louis Dreyfus', 'Beverly -  once again thanks so much for your help on this.
');
INSERT INTO email([from],[to],subject,body) VALUES('karolyn.criado@enron.com', 'j..bonin@enron.com, felicia.case@enron.com, b..clapp@enron.com,', 'Price List week of Oct. 8-9, 2001', '
Please contact me if you have any questions regarding last weeks prices.
Thank you,
Karolyn Criado
3-9441
');
INSERT INTO email([from],[to],subject,body) VALUES('kevin.presto@enron.com', 'edward.baughman@enron.com, billy.braddock@enron.com', 'Associated', 'Please begin working on filling our Associated short position in 02.   I would like to take this risk off the books.
In addition, please find out what a buy-out of VEPCO would cost us.   With Rogers transitioning to run our retail risk management, I would like to clean up our customer positions.
We also need to continue to explore a JEA buy-out.
Thanks.');
INSERT INTO email([from],[to],subject,body) VALUES('stacy.dickson@enron.com', 'gregg.penman@enron.com', 'RE: Constellation TC 5-7-01', 'Gregg, 
I am at home with a sick baby.  (Lots of fun!)  I will call you about this 
tomorrow.
Stacy');
INSERT INTO email([from],[to],subject,body) VALUES('joe.quenet@enron.com', 'dfincher@utilicorp.com', '', 'hey big guy.....check this out.....
w ww.gorelieberman-2000.com/');
INSERT INTO email([from],[to],subject,body) VALUES('k..allen@enron.com', 'jacqestc@aol.com', '', 'Jacques,
I sent you a fax of Kevin Kolb''s comments on the release.  The payoff on the note would be 36,248 (36090(principal) + 158 (accrued interest)).
This is assuming we wrap this up on Tuesday.  
Please email to confirm that their changes are ok so I can set up a meeting on Tuesday to reach closure.
Phillip');
INSERT INTO email([from],[to],subject,body) VALUES('kourtney.nelson@enron.com', 'mike.swerzbin@enron.com', 'Adjusted L/R Balance', 'Mike,
I placed the adjusted L/R Balance on the Enronwest site.  It is under the "Staff/Kourtney Nelson".  There are two links:  
1)  "Adj L_R" is the same data/format from the weekly strategy meeting. 
2)  "New Gen 2001_2002" link has all of the supply side info that is used to calculate the L/R balance
-Please note the Data Flag column, a value of "3" indicates the project was cancelled, on hold, etc and is not included in the calc.  
Both of these sheets are interactive Excel spreadsheets and thus you can play around with the data as you please.  Also, James Bruce is working to get his gen report on the web.  That will help with your access to information on new gen.
Please let me know if you have any questions or feedback,
Kourtney
Kourtney Nelson
Fundamental Analysis 
Enron North America
(503) 464-8280
kourtney.nelson@enron.com');
INSERT INTO email([from],[to],subject,body) VALUES('d..thomas@enron.com', 'naveed.ahmed@enron.com', 'FW: Current Enron TCC Portfolio', '
From: Grace, Rebecca M. 
Sent: Monday, December 17, 2001 9:44 AM
To: Thomas, Paul D.
Cc: Cashion, Jim; Allen, Thresa A.; May, Tom
Subject: RE: Current Enron TCC Portfolio
Paul,
I reviewed NY''s list.  I agree with all of their contracts numbers and mw amounts.
Call if you have any more questions.
Rebecca
From: 	Thomas, Paul D.  
Sent:	Monday, December 17, 2001 9:08 AM
To:	Grace, Rebecca M.
Subject:	FW: Current Enron TCC Portfolio
<< File: enrontccs.xls >> 
Rebecca,
Let me know if you see any differences.
Paul
X 3-0403
From: Thomas, Paul D. 
Sent: Monday, December 17, 2001 9:04 AM
To: Ahmed, Naveed
Subject: FW: Current Enron TCC Portfolio
From: Thomas, Paul D. 
Sent: Thursday, December 13, 2001 10:01 AM
To: Baughman, Edward D.
Subject: Current Enron TCC Portfolio
');
INSERT INTO email([from],[to],subject,body) VALUES('stephanie.panus@enron.com', 'william.bradford@enron.com, debbie.brackett@enron.com,', 'Coastal Merchant Energy/El Paso Merchant Energy', 'Coastal Merchant Energy, L.P. merged with and into El Paso Merchant Energy, 
L.P., effective February 1, 2001, with the surviving entity being El Paso 
Merchant Energy, L.P.  We currently have ISDA Master Agreements with both 
counterparties.  Please see the attached memo regarding the existing Masters 
and let us know which agreement should be terminated.
Thanks,
Stephanie
');
INSERT INTO email([from],[to],subject,body) VALUES('kam.keiser@enron.com', 'c..kenne@enron.com', 'RE: What about this too???', ' 
From: 	Kenne, Dawn C.  
Sent:	Wednesday, February 06, 2002 11:50 AM
To:	Keiser, Kam
Subject:	What about this too???
<< File: Netco Trader Matrix.xls >> 
');
INSERT INTO email([from],[to],subject,body) VALUES('chris.meyer@enron.com', 'joe.parks@enron.com', 'Centana', 'Talked to Chip.  We do need Cash Committe approval given the netting feature of your deal, which means Batch Funding Request.  Please update per my previous e-mail and forward.
Thanks
chris
x31666');
INSERT INTO email([from],[to],subject,body) VALUES('debra.perlingiere@enron.com', 'jworman@academyofhealth.com', '', 'Have a great weekend!   Happy Fathers Day!
Debra Perlingiere
Enron North America Corp.
1400 Smith Street, EB 3885
Houston, Texas 77002
dperlin@enron.com
Phone 713-853-7658
Fax  713-646-3490');
INSERT INTO email([from],[to],subject,body) VALUES('outlook.team@enron.com', '', 'Demo by Martha Janousek of Dashboard & Pipeline Profile / Julia  &', 'CALENDAR ENTRY:	APPOINTMENT
Description:
Demo by Martha Janousek of Dashboard & Pipeline Profile / Julia  & Dir Rpts. - 4102
Date:		1/5/2001
Time:		9:00 AM - 10:00 AM (Central Standard Time)
Chairperson:	Outlook Migration Team
Detailed Description:');
INSERT INTO email([from],[to],subject,body) VALUES('diana.seifert@enron.com', 'mark.taylor@enron.com', 'Guest access Chile', 'Hello Mark,
Justin Boyd told me that your can help me with questions regarding Chile.
We got a request for guest access through MG.
The company is called Escondida and is a subsidiary of BHP Australia.
Please advise if I can set up a guest account or not.
F.Y.I.: MG is planning to put a "in w/h Chile" contract for Copper on-line as 
soon as Enron has done the due diligence for this country.
Thanks !
Best regards
Diana Seifert
EOL PCG');
INSERT INTO email([from],[to],subject,body) VALUES('enron_update@concureworkplace.com', 'mark.whitt@enron.com', '<<Concur Expense Document>> - 121001', 'The Approval status has changed on the following report:
Status last changed by: Barry L. Tycholiz
Expense Report Name: 121001
Report Total: 198.98
Amount Due Employee: 198.98
Amount Approved: 198.98
Amount Paid: 0.00
Approval Status: Approved
Payment Status: Pending
To review this expense report, click on the following link for Concur Expense.
http://expensexms.enron.com');
INSERT INTO email([from],[to],subject,body) VALUES('kevin.hyatt@enron.com', '', 'Technical Support', 'Outside the U.S., please refer to the list below:
Australia:
1800 678-515
support@palm-au.com
Canada:
1905 305-6530
support@palm.com
New Zealand:
0800 446-398
support@palm-nz.com
U.K.:
0171 867 0108
eurosupport@palm.3com.com
Please refer to the Worldwide Customer Support card for a complete technical support contact list.');
INSERT INTO email([from],[to],subject,body) VALUES('geoff.storey@enron.com', 'dutch.quigley@enron.com', 'RE:', 'duke contact?
From: 	Quigley, Dutch  
Sent:	Wednesday, October 31, 2001 10:14 AM
To:	Storey, Geoff
Subject:	RE: 
bp corp	Albert LaMore	281-366-4962
running the reports now
From: 	Storey, Geoff  
Sent:	Wednesday, October 31, 2001 10:10 AM
To:	Quigley, Dutch
Subject:	RE: 
give me a contact over there too
BP
From: 	Quigley, Dutch  
Sent:	Wednesday, October 31, 2001 9:42 AM
To:	Storey, Geoff
Subject:	
Coral	Jeff Whitnah	713-767-5374
Relaint	Steve McGinn	713-207-4000');
INSERT INTO email([from],[to],subject,body) VALUES('pete.davis@enron.com', 'pete.davis@enron.com', 'Start Date: 4/22/01; HourAhead hour: 3;  <CODESITE>', 'Start Date: 4/22/01; HourAhead hour: 3;  No ancillary schedules awarded.  
Variances detected.
Variances detected in Load schedule.
LOG MESSAGES:
PARSING FILE Schedules\2001042203.txt
Variance found in table tblLoads.
Details: (Hour: 3 / Preferred:   1.92 / Final:   1.89)
TRANS_TYPE: FINAL
LOAD_ID: PGE4
MKT_TYPE: 2
TRANS_DATE: 4/22/01
SC_ID: EPMI
');
INSERT INTO email([from],[to],subject,body) VALUES('john.postlethwaite@enron.com', 'john.zufferli@enron.com', 'Reference', 'John, hope things are going well up there for you. The big day is almost here for you and Jessica. I was wondering if I could use your name as a job reference if need be. I am just trying to get everything in order just in case something happens. 
John');
INSERT INTO email([from],[to],subject,body) VALUES('jeffrey.shankman@enron.com', 'lschiffm@jonesday.com', 'Re:', 'I saw you called on the cell this a.m.  Sorry I missed you.  (I was in the 
shower).  I have had a shitty weekbut others) after our phone call is a result of the week.  I''m seeing Glen at 
11:15....talk to you');
INSERT INTO email([from],[to],subject,body) VALUES('litebytz@enron.com', '', 'Lite Bytz RSVP', '
This week''s Lite Bytz presentation will feature the following TOOLZ speaker:
Richard McDougall
Solaris 8
Thursday, June 7, 2001
If you have not already signed up, please RSVP via email to litebytz@enron.com by the end of the day Tuesday, June 5, 2001.
*Remember: this is now a Brown Bag EventClick below for more details.
http://home.enron.com:84/messaging/litebytztoolzprint.jpg');
COMMIT;

SELECT rowid, offsets(email) FROM email
WHERE email MATCH 'gas reminder';

SELECT rowid, offsets(email) FROM email
WHERE email MATCH 'subject:gas reminder';

SELECT rowid, offsets(email) FROM email
WHERE email MATCH 'body:gas reminder';

SELECT rowid, offsets(email) FROM email
WHERE subject MATCH 'gas reminder';

SELECT rowid, offsets(email) FROM email
WHERE body MATCH 'gas reminder';

SELECT rowid, offsets(email) FROM email
WHERE body MATCH 'child product' AND +rowid=32;

SELECT rowid, offsets(email) FROM email
WHERE body MATCH '"child product"';

SELECT snippet(email) FROM email
WHERE email MATCH 'subject:gas reminder';

SELECT snippet(email) FROM email
WHERE email MATCH 'christmas candlelight';

SELECT snippet(email) FROM email
WHERE email MATCH 'deal sheet potential reuse';

SELECT rowid FROM email WHERE email MATCH 'mark';

SELECT snippet(email,'<<<','>>>',' ') FROM email
WHERE email MATCH 'deal sheet potential reuse';

SELECT snippet(email,'<<<','>>>',' ') FROM email
WHERE email MATCH 'first things';

SELECT snippet(email) FROM email
WHERE email MATCH 'chris is here';

SELECT snippet(email) FROM email
WHERE email MATCH '"pursuant to"';

SELECT snippet(email) FROM email
WHERE email MATCH 'ancillary load davis';

SELECT snippet(email) FROM email
WHERE email MATCH 'questar enron OR com';

SELECT snippet(email) FROM email
WHERE email MATCH 'enron OR com questar';

CREATE VIRTUAL TABLE ft USING fts3(one, two);
INSERT INTO ft VALUES('', 'foo');
INSERT INTO ft VALUES('foo', 'foo');
SELECT offsets(ft) FROM ft WHERE ft MATCH 'foo';

DELETE FROM ft WHERE one = 'foo';
SELECT offsets(ft) FROM ft WHERE ft MATCH 'foo';

SELECT rowid FROM email WHERE email MATCH 'susan';

SELECT rowid FROM email WHERE email MATCH 'mark susan';

SELECT rowid FROM email WHERE email MATCH 'susan mark';

SELECT rowid FROM email WHERE email MATCH '"mark susan"';

SELECT rowid FROM email WHERE email MATCH 'mark -susan';

SELECT rowid FROM email WHERE email MATCH '-mark susan';

SELECT rowid FROM email WHERE email MATCH 'mark OR susan';

-- ===fts3ad.test===
CREATE VIRTUAL TABLE t1 USING fts3(content, tokenize porter);
INSERT INTO t1(rowid, content) VALUES(1, 'running and jumping');
SELECT rowid FROM t1 WHERE content MATCH 'run jump';

DROP TABLE t1;
CREATE VIRTUAL TABLE t1 USING fts3(content,   tokenize=   porter);
INSERT INTO t1(rowid, content) VALUES(1, 'running and jumping');
SELECT rowid FROM t1 WHERE content MATCH 'run jump';

DROP TABLE t1;
CREATE VIRTUAL TABLE t1 USING fts3(content,	   tokenize =   porter);
INSERT INTO t1(rowid, content) VALUES(1, 'running and jumping');
SELECT rowid FROM t1 WHERE content MATCH 'run jump';

SELECT snippet(t1) FROM t1 WHERE t1 MATCH 'run jump';

INSERT INTO t1(rowid, content) 
VALUES(2, 'abcdefghijklmnopqrstuvwyxz');
SELECT rowid, snippet(t1) FROM t1 WHERE t1 MATCH 'abcdefghijqrstuvwyxz';

SELECT rowid, snippet(t1) FROM t1 WHERE t1 MATCH 'abcdefghijXXXXqrstuvwyxz';

INSERT INTO t1(rowid, content) 
VALUES(3, 'The value is 123456789');
SELECT rowid, snippet(t1) FROM t1 WHERE t1 MATCH '123789';

SELECT rowid, snippet(t1) FROM t1 WHERE t1 MATCH '123000000789';

DROP TABLE t1;
CREATE VIRTUAL TABLE t1 USING fts3(content, tokenize    porter);
INSERT INTO t1(rowid, content) VALUES(1, 'running and jumping');
SELECT rowid FROM t1 WHERE content MATCH 'run jump';

DROP TABLE t1;
CREATE VIRTUAL TABLE t1 USING fts3(content, tokenize=   porter);
INSERT INTO t1(rowid, content) VALUES(1, 'running and jumping');
SELECT rowid FROM t1 WHERE content MATCH 'run jump';

DROP TABLE t1;
CREATE VIRTUAL TABLE t1 USING fts3(content, tokenize=   simple);
INSERT INTO t1(rowid, content) VALUES(1, 'running and jumping');
SELECT rowid FROM t1 WHERE content MATCH 'run jump';

-- ===fts3ae.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1 (rowid, content) VALUES(1, 'one');
INSERT INTO t1 (rowid, content) VALUES(2, 'two');
INSERT INTO t1 (rowid, content) VALUES(3, 'one two');
INSERT INTO t1 (rowid, content) VALUES(4, 'three');
DELETE FROM t1 WHERE rowid = 1;
INSERT INTO t1 (rowid, content) VALUES(5, 'one three');
INSERT INTO t1 (rowid, content) VALUES(6, 'two three');
INSERT INTO t1 (rowid, content) VALUES(7, 'one two three');
DELETE FROM t1 WHERE rowid = 4;
INSERT INTO t1 (rowid, content) VALUES(8, 'four');
INSERT INTO t1 (rowid, content) VALUES(9, 'one four');
INSERT INTO t1 (rowid, content) VALUES(10, 'two four');
DELETE FROM t1 WHERE rowid = 7;
INSERT INTO t1 (rowid, content) VALUES(11, 'one two four');
INSERT INTO t1 (rowid, content) VALUES(12, 'three four');
INSERT INTO t1 (rowid, content) VALUES(13, 'one three four');
DELETE FROM t1 WHERE rowid = 10;
INSERT INTO t1 (rowid, content) VALUES(14, 'two three four');
INSERT INTO t1 (rowid, content) VALUES(15, 'one two three four');
INSERT INTO t1 (rowid, content) VALUES(16, 'five');
DELETE FROM t1 WHERE rowid = 13;
INSERT INTO t1 (rowid, content) VALUES(17, 'one five');
INSERT INTO t1 (rowid, content) VALUES(18, 'two five');
INSERT INTO t1 (rowid, content) VALUES(19, 'one two five');
DELETE FROM t1 WHERE rowid = 16;
INSERT INTO t1 (rowid, content) VALUES(20, 'three five');
INSERT INTO t1 (rowid, content) VALUES(21, 'one three five');
INSERT INTO t1 (rowid, content) VALUES(22, 'two three five');
DELETE FROM t1 WHERE rowid = 19;
DELETE FROM t1 WHERE rowid = 22;

SELECT COUNT(*) FROM t1;

SELECT rowid FROM t1 WHERE content MATCH 'one';

SELECT rowid FROM t1 WHERE content MATCH 'two';

SELECT rowid FROM t1 WHERE content MATCH 'three';

SELECT rowid FROM t1 WHERE content MATCH 'four';

SELECT rowid FROM t1 WHERE content MATCH 'five';

-- ===fts3af.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1 (rowid, content) VALUES(1, 'one');
INSERT INTO t1 (rowid, content) VALUES(2, 'two');
INSERT INTO t1 (rowid, content) VALUES(3, 'one two');
INSERT INTO t1 (rowid, content) VALUES(4, 'three');
INSERT INTO t1 (rowid, content) VALUES(5, 'one three');
INSERT INTO t1 (rowid, content) VALUES(6, 'two three');
INSERT INTO t1 (rowid, content) VALUES(7, 'one two three');
DELETE FROM t1 WHERE rowid = 4;
INSERT INTO t1 (rowid, content) VALUES(8, 'four');
UPDATE t1 SET content = 'update one three' WHERE rowid = 1;
INSERT INTO t1 (rowid, content) VALUES(9, 'one four');
INSERT INTO t1 (rowid, content) VALUES(10, 'two four');
DELETE FROM t1 WHERE rowid = 7;
INSERT INTO t1 (rowid, content) VALUES(11, 'one two four');
INSERT INTO t1 (rowid, content) VALUES(12, 'three four');
INSERT INTO t1 (rowid, content) VALUES(13, 'one three four');
DELETE FROM t1 WHERE rowid = 10;
INSERT INTO t1 (rowid, content) VALUES(14, 'two three four');
INSERT INTO t1 (rowid, content) VALUES(15, 'one two three four');
UPDATE t1 SET content = 'update two five' WHERE rowid = 8;
INSERT INTO t1 (rowid, content) VALUES(16, 'five');
DELETE FROM t1 WHERE rowid = 13;
INSERT INTO t1 (rowid, content) VALUES(17, 'one five');
INSERT INTO t1 (rowid, content) VALUES(18, 'two five');
INSERT INTO t1 (rowid, content) VALUES(19, 'one two five');
DELETE FROM t1 WHERE rowid = 16;
INSERT INTO t1 (rowid, content) VALUES(20, 'three five');
INSERT INTO t1 (rowid, content) VALUES(21, 'one three five');
INSERT INTO t1 (rowid, content) VALUES(22, 'two three five');
DELETE FROM t1 WHERE rowid = 19;
UPDATE t1 SET content = 'update' WHERE rowid = 15;

SELECT COUNT(*) FROM t1;

SELECT rowid FROM t1 WHERE content MATCH 'update';

SELECT rowid FROM t1 WHERE content MATCH 'one';

SELECT rowid FROM t1 WHERE content MATCH 'two';

SELECT rowid FROM t1 WHERE content MATCH 'three';

SELECT rowid FROM t1 WHERE content MATCH 'four';

SELECT rowid FROM t1 WHERE content MATCH 'five';

-- ===fts3ag.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1 (rowid, content) VALUES(1, 'this is a test');
INSERT INTO t1 (rowid, content) VALUES(2, 'also a test');

SELECT rowid FROM t1 WHERE t1 MATCH 'this something';

SELECT rowid FROM t1 WHERE t1 MATCH 'this OR also';

SELECT rowid FROM t1 WHERE t1 MATCH 'also OR this';

SELECT rowid FROM t1 WHERE t1 MATCH 'something OR nothing';

SELECT rowid FROM t1 WHERE t1 MATCH 'something';

SELECT rowid FROM t1 WHERE t1 MATCH '-this something';

SELECT rowid FROM t1 WHERE t1 MATCH 'this -something';

SELECT rowid FROM t1 WHERE t1 MATCH '"this something"';

SELECT rowid FROM t1 WHERE t1 MATCH '"something is"';

SELECT rowid FROM t1 WHERE t1 MATCH 'something OR this';

SELECT rowid FROM t1 WHERE t1 MATCH 'this OR something';

SELECT rowid FROM t1 WHERE t1 MATCH 'something this';

-- ===fts3ah.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1 (rowid, content) VALUES(1, doc1);
INSERT INTO t1 (rowid, content) VALUES(2, doc2);
INSERT INTO t1 (rowid, content) VALUES(3, doc3);

SELECT rowid FROM t1 WHERE t1 MATCH 'something';

SELECT rowid FROM t1 WHERE t1 MATCH aterm;

SELECT rowid FROM t1 WHERE t1 MATCH xterm;

-- ===fts3ai.test===
PRAGMA encoding = "UTF-16le";
CREATE VIRTUAL TABLE t1 USING fts3(content);

PRAGMA encoding;

INSERT INTO t1 (rowid, content) VALUES(1, 'one');

SELECT content FROM t1 WHERE rowid = 1;

SELECT content FROM t1 WHERE rowid = 2;

SELECT content FROM t1 WHERE rowid = 3;

SELECT content FROM t1 WHERE rowid = 4;

SELECT content FROM t1 WHERE rowid = 5;

-- ===fts3aj.test===
CREATE VIRTUAL TABLE t3 USING fts3(content);
INSERT INTO t3 (rowid, content) VALUES(1, "hello world");

ATTACH DATABASE 'test2.db' AS two;
SELECT rowid FROM t1 WHERE t1 MATCH 'hello';
DETACH DATABASE two;

DETACH DATABASE two;

ATTACH DATABASE 'test2.db' AS two;
CREATE VIRTUAL TABLE two.t2 USING fts3(content);
INSERT INTO t2 (rowid, content) VALUES(1, "hello world");
INSERT INTO t2 (rowid, content) VALUES(2, "hello there");
INSERT INTO t2 (rowid, content) VALUES(3, "cruel world");
SELECT rowid FROM t2 WHERE t2 MATCH 'hello';
DETACH DATABASE two;

DETACH DATABASE two;

ATTACH DATABASE 'test2.db' AS two;
CREATE VIRTUAL TABLE two.t3 USING fts3(content);
INSERT INTO two.t3 (rowid, content) VALUES(2, "hello there");
INSERT INTO two.t3 (rowid, content) VALUES(3, "cruel world");
SELECT rowid FROM two.t3 WHERE t3 MATCH 'hello';
DETACH DATABASE two;

DETACH DATABASE two;

-- ===fts3ak.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1 (rowid, content) VALUES(1, "hello world");
INSERT INTO t1 (rowid, content) VALUES(2, "hello there");
INSERT INTO t1 (rowid, content) VALUES(3, "cruel world");

BEGIN TRANSACTION;
INSERT INTO t1 (rowid, content) VALUES(4, "false world");
INSERT INTO t1 (rowid, content) VALUES(5, "false door");
COMMIT TRANSACTION;
SELECT rowid FROM t1 WHERE t1 MATCH 'world';

BEGIN TRANSACTION;
INSERT INTO t1 (rowid, content) VALUES(6, "another world");
INSERT INTO t1 (rowid, content) VALUES(7, "another test");
SELECT rowid FROM t1 WHERE t1 MATCH 'world';
COMMIT TRANSACTION;

BEGIN TRANSACTION;
INSERT INTO t1 (rowid, content) VALUES(8, "second world");
INSERT INTO t1 (rowid, content) VALUES(9, "second sight");
SELECT rowid FROM t1 WHERE t1 MATCH 'world';
ROLLBACK TRANSACTION;

SELECT rowid FROM t1 WHERE t1 MATCH 'world';

BEGIN TRANSACTION;
INSERT INTO t1 (rowid, content) VALUES(10, "second world");
INSERT INTO t1 (rowid, content) VALUES(11, "second sight");
ROLLBACK TRANSACTION;
SELECT rowid FROM t1 WHERE t1 MATCH 'world';

BEGIN;
INSERT INTO t1 (rowid, content) VALUES(12, "third world");
COMMIT;
SELECT rowid FROM t1 WHERE t1 MATCH 'third';

BEGIN;
INSERT INTO t1 (rowid, content) VALUES(13, "third dimension");
CREATE TABLE x (c);
COMMIT;
SELECT rowid FROM t1 WHERE t1 MATCH 'dimension';

-- ===fts3al.test===
CREATE VIRTUAL TABLE t4 USING fts3(content);

SELECT rowid, length(snippet(t4)) FROM t4 WHERE t4 MATCH 'target';

-- ===fts3am.test===
CREATE VIRTUAL TABLE t1 USING fts3(col_a, col_b);
INSERT INTO t1(rowid, col_a, col_b) VALUES(1, 'testing', 'testing');
INSERT INTO t1(rowid, col_a, col_b) VALUES(2, 'only a', null);
INSERT INTO t1(rowid, col_a, col_b) VALUES(3, null, 'only b');
INSERT INTO t1(rowid, col_a, col_b) VALUES(4, null, null);

SELECT COUNT(col_a), COUNT(col_b), COUNT(*) FROM t1;

DELETE FROM t1 WHERE rowid = 1;
SELECT COUNT(col_a), COUNT(col_b), COUNT(*) FROM t1;

DELETE FROM t1 WHERE rowid = 2;
SELECT COUNT(col_a), COUNT(col_b), COUNT(*) FROM t1;

DELETE FROM t1 WHERE rowid = 3;
SELECT COUNT(col_a), COUNT(col_b), COUNT(*) FROM t1;

DELETE FROM t1 WHERE rowid = 4;
SELECT COUNT(col_a), COUNT(col_b), COUNT(*) FROM t1;

-- ===fts3an.test===
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1(rowid, c) VALUES(1, text);
INSERT INTO t1(rowid, c) VALUES(2, 'Another lovely row');

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

CREATE VIRTUAL TABLE t2 USING fts3(c);
INSERT INTO t2(rowid, c) VALUES(1, text);
INSERT INTO t2(rowid, c) VALUES(2, 'Another lovely row');
UPDATE t2 SET c = ntext WHERE rowid = 1;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

INSERT INTO ft SELECT * FROM ft;

UPDATE ft SET x = 'abc' || rowid;

SELECT count(*) FROM ft WHERE x MATCH 'abc*';

BEGIN;
CREATE VIRTUAL TABLE t3 USING fts3(c);
INSERT INTO t3(rowid, c) VALUES(1, text);
INSERT INTO t3(rowid, c) VALUES(2, 'Another lovely row');

INSERT INTO t3(rowid, c) VALUES(3+i, bigtext);

COMMIT;

SELECT offsets(t3) as o FROM t3 WHERE t3 MATCH 'l*';

CREATE VIRTUAL TABLE ft USING fts3(x);

INSERT INTO ft VALUES(NULL);

INSERT INTO ft SELECT * FROM ft;

-- ===fts3ao.test===
CREATE VIRTUAL TABLE t1 USING fts3(a, b, c);
INSERT INTO t1(a, b, c) VALUES('one three four', 'one four', 'one four two');

SELECT tbl_name FROM sqlite_master WHERE type = 'table';

BEGIN;
INSERT INTO fts_t1(a, b, c) VALUES('one two three', 'one four', 'one two');

SELECT rowid, snippet(fts_t1) FROM fts_t1 WHERE a MATCH 'four';

SELECT tbl_name FROM sqlite_master WHERE type = 'table';

SELECT a FROM fts_t1;

SELECT a, b, c FROM fts_t1 WHERE c MATCH 'four';

CREATE VIRTUAL TABLE t1 USING fts3(a, b, c);
INSERT INTO t1(a, b, c) VALUES('one three four', 'one four', 'one two');
SELECT a, b, c FROM t1 WHERE c MATCH 'two';

SELECT a, b, c FROM t1 WHERE c MATCH 'two';
CREATE TABLE t3(a, b, c);
SELECT a, b, c FROM t1 WHERE  c  MATCH 'two';

ATTACH 'test2.db' AS aux;
CREATE VIRTUAL TABLE aux.t1 USING fts3(a, b, c);
INSERT INTO aux.t1(a, b, c) VALUES(
'neung song sahm', 'neung see', 'neung see song'
);

SELECT a, b, c FROM aux.t1 WHERE a MATCH 'song';

SELECT rowid, snippet(t1) FROM t1 WHERE c MATCH 'four';

SELECT a, b, c FROM t1 WHERE c MATCH 'two';

ALTER TABLE aux.t1 RENAME TO t2;

SELECT a, b, c FROM t2 WHERE a MATCH 'song';

SELECT a, b, c FROM t1 WHERE c MATCH 'two';

CREATE VIRTUAL TABLE t4 USING fts3;
INSERT INTO t4 VALUES('the quick brown fox');

BEGIN;
INSERT INTO t4 VALUES('jumped over the');

ALTER TABLE t4 RENAME TO t5;

INSERT INTO t5 VALUES('lazy dog');

SELECT * FROM t5;

BEGIN;
INSERT INTO t5 VALUES('Down came a jumbuck to drink at that billabong');
ALTER TABLE t5 RENAME TO t6;
INSERT INTO t6 VALUES('Down came the troopers, one, two, three');
ROLLBACK;
SELECT * FROM t5;

SELECT rowid, snippet(t1) FROM t1 WHERE b MATCH 'four';

SELECT rowid, snippet(t1) FROM t1 WHERE a MATCH 'four';

SELECT tbl_name FROM sqlite_master WHERE type = 'table';

ALTER TABLE t1 RENAME to fts_t1;

SELECT rowid, snippet(fts_t1) FROM fts_t1 WHERE a MATCH 'four';

SELECT tbl_name FROM sqlite_master WHERE type = 'table';

SELECT rowid, snippet(fts_t1) FROM fts_t1 WHERE a MATCH 'four';

-- ===fts3atoken.test===
SELECT fts3_tokenizer('blah', fts3_tokenizer('simple')) IS NULL;

SELECT fts3_tokenizer('blah') == fts3_tokenizer('simple');

INSERT INTO t1(content) VALUES('There was movement at the station');
INSERT INTO t1(content) VALUES('For the word has passed around');
INSERT INTO t1(content) VALUES('That the colt from ol regret had got away');
SELECT content FROM t1 WHERE content MATCH 'movement';

SELECT fts3_tokenizer_test('simple', 'I don''t see how');

SELECT fts3_tokenizer_test('porter', 'I don''t see how');

SELECT fts3_tokenizer_test('icu', 'I don''t see how');

SELECT fts3_tokenizer_test('icu', locale, input);

SELECT fts3_tokenizer_internal_test();

-- ===fts3b.test===
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (c) VALUES('this is a test');
INSERT INTO t1 (c) VALUES('that was a test');
INSERT INTO t1 (c) VALUES('this is fun');
DELETE FROM t1 WHERE c = 'that was a test';

SELECT rowid FROM t2 WHERE c MATCH 'lorem';

VACUUM;

SELECT rowid FROM t2 WHERE c MATCH 'lorem';

CREATE VIRTUAL TABLE t3 USING fts3(c);
INSERT INTO t3 (c) VALUES('this is a test');
INSERT INTO t3 (c) VALUES('that was a test');
INSERT INTO t3 (c) VALUES('this is fun');
DELETE FROM t3 WHERE c = 'that was a test';

SELECT snippet(t3) FROM t3 WHERE t3 MATCH 'test';

SELECT * FROM t3 WHERE rowid = 1;

INSERT INTO t3 VALUES ('another test');

CREATE VIRTUAL TABLE t4 USING fts3(c);
INSERT INTO t4 (c) VALUES('this is a test');
INSERT INTO t4 (c) VALUES('that was a test');
INSERT INTO t4 (c) VALUES('this is fun');
DELETE FROM t4 WHERE c = 'that was a test';

SELECT rowid FROM t4 WHERE rowid <> docid;

SELECT * FROM t4 WHERE rowid = 1;

SELECT rowid FROM t1 WHERE c MATCH 'this';

SELECT docid, * FROM t4 WHERE rowid = 1;

SELECT docid, * FROM t4 WHERE docid = 1;

INSERT INTO t4 VALUES ('another test');

INSERT INTO t4 (docid, c) VALUES (10, 'yet another test');
SELECT * FROM t4 WHERE docid = 10;

INSERT INTO t4 (docid, c) VALUES (12, 'still testing');
SELECT * FROM t4 WHERE docid = 12;

SELECT docid FROM t4 WHERE t4 MATCH 'testing';

UPDATE t4 SET docid = 14 WHERE docid = 12;
SELECT docid FROM t4 WHERE t4 MATCH 'testing';

SELECT * FROM t4 WHERE rowid = 14;

SELECT * FROM t4 WHERE rowid = 12;

SELECT docid FROM t4 WHERE t4 MATCH 'still';

VACUUM;

SELECT rowid FROM t1 WHERE c MATCH 'this';

CREATE VIRTUAL TABLE t2 USING fts3(c);

BEGIN;

INSERT INTO t2 (c) VALUES (text);

COMMIT;
BEGIN;

COMMIT;

-- ===fts3c.test===
DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');

DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');
DELETE FROM t1 WHERE docid IN (1,3);
DROP TABLE IF EXISTS t1old;
ALTER TABLE t1 RENAME TO t1old;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) SELECT docid, c FROM t1old;
DROP TABLE t1old;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');
DELETE FROM t1 WHERE docid = 1;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');
DELETE FROM t1 WHERE docid IN (1,3);

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

-- ===fts3corrupt.test===
UPDATE t1_segdir SET root = blob;

INSERT INTO t1 VALUES(w);

UPDATE t1_segdir SET root = blob;

UPDATE t1_segdir SET root = blob;

INSERT INTO t1 VALUES(s);

-- ===fts3corrupt2.test===
CREATE VIRTUAL TABLE t2 USING FTS3(a, b);

UPDATE t2_segdir SET root = blob WHERE rowid = rowid;

INSERT INTO t2(t2) VALUES('nodesize=32');

INSERT INTO t2 VALUES(d, d);

SELECT count(*) FROM t2_segments;

SELECT rowid, length(block), block FROM t2_segments;

UPDATE t2_segments SET block = b2 WHERE rowid = rowid;

UPDATE t2_segments SET block = blob WHERE rowid = rowid;

SELECT rowid, length(root), root FROM t2_segdir;

UPDATE t2_segdir SET root = b2 WHERE rowid = rowid;

-- ===fts3cov.test===
CREATE VIRTUAL TABLE t1 USING fts3(x);
INSERT INTO t1(t1) VALUES('nodesize=24');
BEGIN;
INSERT INTO t1 VALUES('Is the night chilly and dark?');
INSERT INTO t1 VALUES('The night is chilly, but not dark.');
INSERT INTO t1 VALUES('The thin gray cloud is spread on high,');
INSERT INTO t1 VALUES('It covers but not hides the sky.');
COMMIT;
SELECT count(*)>0 FROM t1_segments;

CREATE VIRTUAL TABLE t5 USING fts3(x);

SELECT count(*) FROM t5_segdir;

SELECT count(*) FROM t5_segdir;

CREATE VIRTUAL TABLE t7 USING fts3(a, b, c);
INSERT INTO t7 VALUES('A', 'B', 'C');
UPDATE t7 SET docid = 5;
SELECT docid, * FROM t7;

INSERT INTO t7 VALUES('D', 'E', 'F');
UPDATE t7 SET docid = 1 WHERE docid = 6;
SELECT docid, * FROM t7;

CREATE VIRTUAL TABLE xx USING fts3;

INSERT INTO xx(xx) VALUES('optimize');

CREATE VIRTUAL TABLE xx USING fts3;
INSERT INTO xx VALUES('one two three');
INSERT INTO xx VALUES('four five six');
DELETE FROM xx WHERE docid = 1;

SELECT * FROM xx WHERE xx MATCH 'two';

INSERT INTO t1(t1) VALUES('nodesize=24');
BEGIN;
INSERT INTO t1 VALUES('The moon is behind, and at the full;');
INSERT INTO t1 VALUES('And yet she looks both small and dull.');
INSERT INTO t1 VALUES('The night is chill, the cloud is gray:');
INSERT INTO t1 VALUES('''T is a month before the month of May,');
INSERT INTO t1 VALUES('And the Spring comes slowly up this way.');
INSERT INTO t1 VALUES('The lovely lady, Christabel,');
INSERT INTO t1 VALUES('Whom her father loves so well,');
INSERT INTO t1 VALUES('What makes her in the wood so late,');
INSERT INTO t1 VALUES('A furlong from the castle gate?');
INSERT INTO t1 VALUES('She had dreams all yesternight');
INSERT INTO t1 VALUES('Of her own betrothed knight;');
INSERT INTO t1 VALUES('And she in the midnight wood will pray');
INSERT INTO t1 VALUES('For the weal of her lover that''s far away.');
COMMIT;

INSERT INTO t1(t1) VALUES('optimize');
SELECT substr(hex(root), 1, 2) FROM t1_segdir;

DELETE FROM t1_segments WHERE blockid = left_child;

INSERT INTO t1_segments VALUES(left_child, NULL);

CREATE VIRTUAL TABLE t3 USING fts3(x);
INSERT INTO t3(t3) VALUES('nodesize=24');
INSERT INTO t3(t3) VALUES('maxpending=100');

CREATE VIRTUAL TABLE t4 USING fts3(x);
INSERT INTO t4(t4) VALUES('nodesize=24');

INSERT INTO t4 VALUES('extra!');

INSERT INTO t4 VALUES('more extra!');

-- ===fts3d.test===
DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');
DELETE FROM t1 WHERE 1=1; INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');

DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (rowid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (rowid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (rowid, c) VALUES (3, 'This is a test');
UPDATE t1 SET c = 'This is a test one' WHERE rowid = 1;
UPDATE t1 SET c = 'That was a test one' WHERE rowid = 2;
UPDATE t1 SET c = 'This is a test one' WHERE rowid = 3;
UPDATE t1 SET c = 'This is a test two' WHERE rowid = 1;
UPDATE t1 SET c = 'That was a test two' WHERE rowid = 2;
UPDATE t1 SET c = 'This is a test two' WHERE rowid = 3;
UPDATE t1 SET c = 'This is a test three' WHERE rowid = 1;
UPDATE t1 SET c = 'That was a test three' WHERE rowid = 2;
UPDATE t1 SET c = 'This is a test three' WHERE rowid = 3;
UPDATE t1 SET c = 'This is a test four' WHERE rowid = 1;
UPDATE t1 SET c = 'That was a test four' WHERE rowid = 2;
UPDATE t1 SET c = 'This is a test four' WHERE rowid = 3;
UPDATE t1 SET c = 'This is a test' WHERE rowid = 1;
UPDATE t1 SET c = 'That was a test' WHERE rowid = 2;
UPDATE t1 SET c = 'This is a test' WHERE rowid = 3;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

SELECT OPTIMIZE(t1) FROM t1 LIMIT 1;
SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

SELECT OPTIMIZE(t1) FROM t1 LIMIT 1;
SELECT level, idx FROM t1_segdir ORDER BY level, idx;

UPDATE t1_segdir SET level = 2 WHERE level = 1 AND idx = 0;
SELECT OPTIMIZE(t1) FROM t1 LIMIT 1;
SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');
DELETE FROM t1 WHERE docid IN (1,3);
DROP TABLE IF EXISTS t1old;
ALTER TABLE t1 RENAME TO t1old;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) SELECT docid, c FROM t1old;
DROP TABLE t1old;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');
DELETE FROM t1 WHERE docid IN (1,3);
SELECT OPTIMIZE(t1) FROM t1 LIMIT 1;

SELECT level, idx FROM t1_segdir ORDER BY level, idx;

SELECT OFFSETS(t1) FROM t1
WHERE t1 MATCH 'this OR that OR was OR a OR is OR test' ORDER BY docid;

-- ===fts3defer.test===
INSERT INTO t1 VALUES('');

CREATE VIRTUAL TABLE t1 USING FTS4(matchinfo=fts3);

INSERT INTO t1 VALUES(doc);

DROP TABLE IF EXISTS t1;

CREATE VIRTUAL TABLE t1 USING FTS3;

INSERT INTO t1 VALUES(doc);

CREATE VIRTUAL TABLE t1 USING FTS4;

INSERT INTO t1 VALUES(doc);

CREATE VIRTUAL TABLE t1 USING FTS4;

INSERT INTO t1 VALUES(doc);

CREATE VIRTUAL TABLE t1 USING FTS4;

INSERT INTO t1 VALUES(doc);

-- ===fts3e.test===
DROP TABLE IF EXISTS t1;
CREATE VIRTUAL TABLE t1 USING fts3(c);
INSERT INTO t1 (docid, c) VALUES (1, 'This is a test');
INSERT INTO t1 (docid, c) VALUES (2, 'That was a test');
INSERT INTO t1 (docid, c) VALUES (3, 'This is a test');

SELECT docid FROM t1 WHERE t1 MATCH 'this' ORDER BY docid;

SELECT docid, weight FROM t1, t2
WHERE t1 MATCH 'this' AND t1.docid = t2.id ORDER BY weight;

SELECT docid FROM t1 ORDER BY docid;

SELECT docid FROM t1 WHERE c LIKE '%test' ORDER BY docid;

SELECT docid FROM t1 WHERE c LIKE 'That%' ORDER BY docid;

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE VIRTUAL TABLE t1 USING fts3(c);
CREATE TABLE t2(id INTEGER PRIMARY KEY AUTOINCREMENT, weight INTEGER UNIQUE);
INSERT INTO t2 VALUES (null, 10);
INSERT INTO t1 (docid, c) VALUES (last_insert_rowid(), 'This is a test');
INSERT INTO t2 VALUES (null, 5);
INSERT INTO t1 (docid, c) VALUES (last_insert_rowid(), 'That was a test');
INSERT INTO t2 VALUES (null, 20);
INSERT INTO t1 (docid, c) VALUES (last_insert_rowid(), 'This is a test');

SELECT docid FROM t1 WHERE docid in (1, 2, 10);
SELECT rowid FROM t1 WHERE rowid in (1, 2, 10);

SELECT docid, weight FROM t1, t2 WHERE t2.id = t1.docid ORDER BY weight;
SELECT t1.rowid, weight FROM t1, t2 WHERE t2.id = t1.rowid ORDER BY weight;

SELECT docid, weight FROM t1, t2
WHERE t2.weight>5 AND t2.id = t1.docid ORDER BY weight;
SELECT t1.rowid, weight FROM t1, t2
WHERE t2.weight>5 AND t2.id = t1.rowid ORDER BY weight;

DROP TABLE IF EXISTS t1;
DROP TABLE IF EXISTS t2;
CREATE VIRTUAL TABLE t1 USING fts3(c);
CREATE TABLE t2(id INTEGER PRIMARY KEY AUTOINCREMENT, weight INTEGER UNIQUE);
INSERT INTO t2 VALUES (null, 10);
INSERT INTO t1 (docid, c) VALUES (last_insert_rowid(), 'This is a test');
INSERT INTO t2 VALUES (null, 5);
INSERT INTO t1 (docid, c) VALUES (last_insert_rowid(), 'That was a test');
INSERT INTO t2 VALUES (null, 20);
INSERT INTO t1 (docid, c) VALUES (last_insert_rowid(), 'This is a test');

-- ===fts3expr.test===
CREATE VIRTUAL TABLE t1 USING fts3(a, b, c);

CREATE VIRTUAL TABLE t1 USING fts3(a);

INSERT INTO t1 VALUES(v);

SELECT rowid FROM t1 WHERE t1 MATCH 'five four one' ORDER BY rowid;

SELECT rowid FROM t1 WHERE t1 MATCH expr ORDER BY rowid;

SELECT rowid FROM t1 WHERE t1 MATCH expr ORDER BY rowid;

CREATE VIRTUAL TABLE test USING fts3 (keyword);
INSERT INTO test VALUES ('abc');
SELECT * FROM test WHERE keyword MATCH '""';

-- ===fts3fault.test===
BEGIN;
INSERT INTO t1 VALUES('registers the FTS3 module');
INSERT INTO t1 VALUES('various support functions');

CREATE VIRTUAL TABLE t4 USING fts4; 
INSERT INTO t4 VALUES('The British Government called on');
INSERT INTO t4 VALUES('as pesetas then became much');

SELECT content FROM t4;

SELECT optimize(t4) FROM t4 LIMIT 1;

CREATE VIRTUAL TABLE t5 USING fts4; 
INSERT INTO t5 VALUES('The British Government called on');
INSERT INTO t5 VALUES('as pesetas then became much');

BEGIN;
INSERT INTO t5 VALUES('influential in shaping his future outlook');
INSERT INTO t5 VALUES('might be acceptable to the British electorate');

SELECT rowid FROM t5 WHERE t5 MATCH 'british';

CREATE VIRTUAL TABLE t6 USING fts4;

SELECT rowid FROM t6;

DROP TABLE t6;

CREATE VIRTUAL TABLE t1 USING fts4(a, b, matchinfo=fts3);

ALTER TABLE t1 RENAME TO t2;

CREATE VIRTUAL TABLE t1 USING fts4(a, b, matchinfo=fs3);

CREATE VIRTUAL TABLE t1 USING fts4(a, b, matchnfo=fts3);

CREATE VIRTUAL TABLE t8 USING fts4;

SELECT mit(matchinfo(t8, 'x')) FROM t8 WHERE t8 MATCH 'a b c';

SELECT mit(matchinfo(t8, 's')) FROM t8 WHERE t8 MATCH 'a b c';

SELECT mit(matchinfo(t8, 'a')) FROM t8 WHERE t8 MATCH 'a b c';

SELECT mit(matchinfo(t8, 'l')) FROM t8 WHERE t8 MATCH 'a b c';

CREATE VIRTUAL TABLE t9 USING fts4(tokenize=porter);
INSERT INTO t9 VALUES(
'this record is used toooooooooooooooooooooooooooooooooooooo try to'
);
SELECT offsets(t9) FROM t9 WHERE t9 MATCH 'to*';

SELECT offsets(t9) FROM t9 WHERE t9 MATCH 'to*';

CREATE VIRTUAL TABLE t3 USING fts4;

INSERT INTO t3(t3) VALUES('nodesize=50');

BEGIN;

INSERT INTO t3 VALUES('aaa' || i);

COMMIT;

SELECT * FROM t3 WHERE t3 MATCH 'x';

SELECT count(rowid) FROM t3 WHERE t3 MATCH 'aa*';

-- ===fts3malloc.test===
ATTACH 'test2.db' AS aux;

DROP TABLE ft1;
DROP TABLE ft2;
DROP TABLE ft3;
DROP TABLE ft4;
DROP TABLE ft6;

CREATE VIRTUAL TABLE ft USING fts3(a, b);

INSERT INTO ft VALUES(a, b);

INSERT INTO ft VALUES(a, b);

DELETE FROM ft WHERE docid>=32;

SELECT a FROM ft;

CREATE VIRTUAL TABLE ft8 USING fts3(x, tokenize porter);

-- ===fts3near.test===
CREATE VIRTUAL TABLE t1 USING fts3(content);
INSERT INTO t1(content) VALUES('one three four five');
INSERT INTO t1(content) VALUES('two three four five');
INSERT INTO t1(content) VALUES('one two three four five');

SELECT docid FROM t1 WHERE content MATCH '"four five" NEAR/0 three';

SELECT docid FROM t1 WHERE content MATCH '"four five" NEAR/2 one';

SELECT docid FROM t1 WHERE content MATCH '"four five" NEAR/1 one';

SELECT docid FROM t1 WHERE content MATCH 'five NEAR/1 "two three"';

SELECT docid FROM t1 WHERE content MATCH 'one NEAR five';

SELECT docid FROM t1 WHERE content MATCH 'four NEAR four';

SELECT docid FROM t1 WHERE content MATCH 'one NEAR two NEAR one';

SELECT docid FROM t1 WHERE content MATCH '"one three" NEAR/0 "four five"';

SELECT docid FROM t1 WHERE content MATCH '"four five" NEAR/0 "one three"';

INSERT INTO t1(content) VALUES('A X B C D A B');

SELECT docid FROM t1 WHERE content MATCH 'one NEAR/0 three';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'A NEAR/0 B';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'B NEAR/0 A';

SELECT offsets(t1) FROM t1 WHERE content MATCH '"C D" NEAR/0 A';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'A NEAR/0 "C D"';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'A NEAR A';

INSERT INTO t1 VALUES('A A A');
SELECT offsets(t1) FROM t1 WHERE content MATCH 'A NEAR/2 A';

DELETE FROM t1;
INSERT INTO t1 VALUES('A A A A');
SELECT offsets(t1) FROM t1 WHERE content MATCH 'A NEAR A NEAR A';

DELETE FROM t1;
INSERT INTO t1(content) VALUES(
'one two three two four six three six nine four eight twelve'
);

SELECT offsets(t1) FROM t1 WHERE content MATCH 'three NEAR/1 one';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'one NEAR/1 three';

SELECT docid FROM t1 WHERE content MATCH 'one NEAR/1 two';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'three NEAR/1 two';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'three NEAR/2 two';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'two NEAR/2 three';

SELECT offsets(t1) FROM t1 WHERE content MATCH 'three NEAR/0 "two four"';

SELECT offsets(t1) FROM t1 WHERE content MATCH '"two four" NEAR/0 three';

INSERT INTO t1(content) VALUES('
This specification defines Cascading Style Sheets, level 2 (CSS2). CSS2 is a style sheet language that allows authors and users to attach style (e.g., fonts, spacing, and aural cues) to structured documents (e.g., HTML documents and XML applications). By separating the presentation style of documents from the content of documents, CSS2 simplifies Web authoring and site maintenance.
CSS2 builds on CSS1 (see [CSS1]) and, with very few exceptions, all valid CSS1 style sheets are valid CSS2 style sheets. CSS2 supports media-specific style sheets so that authors may tailor the presentation of their documents to visual browsers, aural devices, printers, braille devices, handheld devices, etc. This specification also supports content positioning, downloadable fonts, table layout, features for internationalization, automatic counters and numbering, and some properties related to user interface.
');

SELECT snippet(t1) FROM t1 WHERE content MATCH 'specification NEAR supports';

SELECT docid FROM t1 WHERE content MATCH 'specification attach';

SELECT docid FROM t1 WHERE content MATCH 'specification NEAR attach';

SELECT docid FROM t1 WHERE content MATCH 'specification NEAR/18 attach';

SELECT docid FROM t1 WHERE content MATCH 'one NEAR/1 three';

SELECT docid FROM t1 WHERE content MATCH 'specification NEAR/19 attach';

SELECT docid FROM t1 WHERE content MATCH 'specification NEAR/000018 attach';

SELECT docid FROM t1 WHERE content MATCH 'specification NEAR/000019 attach';

INSERT INTO t1 VALUES('
abbrev aberrations abjurations aboding abr abscesses absolutistic
abstention abuses acanthuses acceptance acclaimers accomplish
accoutring accusation acetonic acid acolytes acquitting acrylonitrile
actives acyclic addicted adenoid adjacently adjusting admissible
adoption adulated advantaging advertisers aedes aerogramme aetiology
affiliative afforest afterclap agamogenesis aggrade agings agonize
agron ailurophile airfreight airspeed alarmists alchemizing
alexandrines alien aliped all allergenic allocator allowances almost
alphabetizes altho alvine amaurosis ambles ameliorate amicability amnio
amour ampicillin amusement anadromous analogues anarchy anchormen
anecdota aneurin angst animating anlage announcements anodized
answerable antemeridian anthracene antiabortionist anticlimaxes
antifriction antimitotic antiphon antiques antithetic anviled
apatosaurus aphrodisia apodal aposiopesis apparatus appendectomies
applications appraisingly appropriate apteryx arabinose
arboricultural archdeaconates archipelago ardently arguers armadillo
arnicas arrayed arrowy arthroscope artisans ascensive ashier
aspersorium assail assentor assignees assonants astereognosis
astringency astutest atheistical atomize attachment attenuates
attrahent audibility augite auricle auteurists autobus autolysis
autosome avenge avidest aw awl ayes babirusa backbeats backgrounder
backseat backswings baddie bagnios baked balefuller ballista balmily
bandbox bandylegged bankruptcy baptism barbering bargain barneys
barracuda barterer bashes bassists bathers batterer bavardage
beachfront beanstalk beauteous become bedim bedtimes beermats begat
begun belabors bellarmine belongings bending benthos bereavements
besieger bestialized betide bevels biases bicarbonates bidentate bigger
bile billow bine biodynamics biomedicine biotites birding bisection
bitingly bkg blackheads blaeberry blanking blatherer bleeper blindage
blithefulness blockish bloodstreams bloused blubbing bluestocking
blurted boatbill bobtailed boffo bold boltrope bondservant bonks
bookbinding bookworm booting borating boscages botchers bougainvillea
bounty bowlegged boyhood bracketed brainstorm brandishes
braunschweigers brazilin breakneck breathlessness brewage bridesmaids
brighter brisker broader brokerages bronziest browband brunets bryology
bucking budlike bugleweed bulkily bulling bummer bunglers bureau burgs
burrito bushfire buss butlery buttressing bylines cabdriver cached
cadaverousnesses cafeterias cakewalk calcifies calendula callboy calms
calyptra camisoles camps candelabrum caned cannolis canoodling cantors
cape caponize capsuling caracoled carbolics carcase carditis caretakers
carnallite carousel carrageenan cartels carves cashbook castanets
casuistry catalyzer catchers categorizations cathexis caucuses
causeway cavetto cede cella cementite centenary centrals ceramics ceria
cervixes chafferer chalcopyrites chamfers change chaotically
characteristically charivari chases chatterer cheats cheeks chef
chemurgy chetah chickaree chigoes chillies chinning chirp chive
chloroforms chokebore choplogic chorioids chromatic chronically
chubbiest chunder chutzpah cimetidine cinque circulated circumscribe
cirrose citrin claddagh clamorousness clapperboards classicalism
clauses cleanse clemency clicker clinchers cliquiest clods closeting
cloudscape clucking cnidarian coalfish coatrack coca cockfights coddled
coeducation coexistence cognitively coiffed colatitude collage
collections collinear colonelcy colorimetric columelliform combos
comforters commence commercialist commit commorancy communized compar
compendiously complainers compliance composition comprised comradery
concelebrants concerted conciliation concourses condensate
condonations confab confessionals confirmed conforming congeal
congregant conjectured conjurers connoisseurs conscripting
conservator consolable conspired constricting consuls contagious
contemporaneity contesters continuities contractors contrarian
contrive convalescents convents convexly convulsed cooncan coparcenary
coprolite copyreader cordially corklike cornflour coroner corralling
corrigible corsages cosies cosmonauts costumer cottontails counselings
counterclaim counterpane countertenors courageously couth coveting
coworker cozier cracklings crampon crappies craved cream credenzas
crematoriums cresol cricoid crinkle criterion crocodile crore crossover
crowded cruelest crunch cruzeiros cryptomeria cubism cuesta culprit
cumquat cupped curdle curly cursoring curvy customized cutting cyclamens
cylindrical cytaster dachshund daikon damages damselfly dangling
darkest databanks dauphine dazzling deadpanned deathday debauchers
debunking decameter decedents decibel decisions declinations
decomposition decoratively decretive deduct deescalated defecating
deferentially definiendum defluxion defrocks degrade deice dekaliters
deli delinquencies deludedly demarcates demineralizers demodulating
demonstrabilities demurred deniabilities denouncement denudation
departure deplorable deposing depredatory deputizes derivational
desalinization descriptors desexes desisted despising destitute
detectability determiner detoxifying devalued devilries devotions
dextrous diagenesis dialling diaphoresis diazonium dickeys diddums
differencing dig dignified dildo dimetric dineric dinosaurs diplodocus
directer dirty disagrees disassembler disburses disclosures
disconcerts discountability discrete disembarrass disenthrone
disgruntled dishpans disintegrators dislodged disobedient
dispassionate dispiritednesses dispraised disqualifying
dissatisfying dissidence dissolvers distich distracting distrusts
ditto diverse divineness dizzily dockyard dodgers doggish doited dom
dominium doohickey doozie dorsum doubleheaders dourer downbeats
downshifted doyennes draftsman dramatic drawling dredge drifter
drivelines droopier drowsed drunkards dubiosities duding dulcifying
dumpcart duodecillion durable duteous dyed dysgenic eagles earplugs
earwitness ebonite echoers economical ectothermous edibility educates
effected effigies eggbeaters egresses ejaculates elasticize elector
electrodynamometer electrophorus elem eligibly eloped emaciating
embarcaderos embezzlers embosses embryectomy emfs emotionalizing
empiricist emu enamels enchained encoded encrusts endeavored endogamous
endothelioma energizes engager engrosses enl enologist enrolls ensphere
enters entirety entrap entryways envies eosinophil epicentral
epigrammatized episodic epochs equestrian equitably erect ernes
errorless escalated eschatology espaliers essonite estop eternity
ethnologically eudemonics euphonious euthenist evangelizations
eventuality evilest evulsion examinee exceptionably exciter
excremental execrably exemplars exhalant exhorter exocrine exothermic
expected expends explainable exploratory expostulatory expunges
extends externals extorts extrapolative extrorse eyebolt eyra
facetiously factor faeries fairings fallacies falsities fancifulness
fantasticalness farmhouse fascinate fatalistically fattener fave
fearlessly featly federates feints fellowman fencers ferny
fertilenesses feta feudality fibers fictionalize fiefs fightback
filefish filmier finaglers fingerboards finochio firefly firmament
fishmeal fitted fjords flagitiousnesses flamen flaps flatfooting
flauntier fleapit fleshes flickertail flints floaty floorboards
floristic flow fluffily fluorescein flutes flyspecks foetal folderols
followable foolhardier footlockers foppish forceless foredo foreknows
foreseeing foretaste forgather forlorn formidableness fortalice
forwarding founding foxhunting fragmentarily frangipani fray freeform
freezable freshening fridges frilliest frizzed frontbench frottages
fruitcake fryable fugleman fulminated functionalists fungoid furfuran
furtive fussy fwd gadolinium galabias gallinaceous galvanism gamers
gangland gaoling garganey garrisoning gasp gate gauger gayety geed
geminately generalissimos genii gentled geochronology geomorphic
geriatricians gesellschaft ghat gibbeting giggles gimps girdlers
glabella glaive glassfuls gleefully glistered globetrotted glorifier
gloving glutathione glyptodont goaled gobsmacked goggliest golliwog
goobers gooseberries gormandizer gouramis grabbier gradually grampuses
grandmothers granulated graptolite gratuitously gravitates greaten
greenmailer greys grills grippers groan gropingly grounding groveling
grueled grunter guardroom guggle guineas gummed gunnysacks gushingly
gutturals gynecoid gyrostabilizer habitudes haemophilia hailer hairs
halest hallow halters hamsters handhelds handsaw hangup haranguer
hardheartedness harlotry harps hashing hated hauntingly hayrack
headcases headphone headword heartbreakers heaters hebephrenia
hedonist heightening heliozoan helots hemelytron hemorrhagic hent
herbicides hereunto heroines heteroclitics heterotrophs hexers
hidebound hies hightails hindmost hippopotomonstrosesquipedalian
histologist hittable hobbledehoys hogans holdings holocrine homegirls
homesteader homogeneousness homopolar honeys hoodwinks hoovered
horizontally horridness horseshoers hospitalization hotdogging houri
housemate howitzers huffier humanist humid humors huntress husbandmen
hyaenas hydride hydrokinetics hydroponically hygrothermograph
hyperbolically hypersensitiveness hypnogogic hypodermically
hypothermia iatrochemistry ichthyological idealist ideograms idling
igniting illegal illuminatingly ilmenite imbibing immateriality
immigrating immortalizes immures imparts impeder imperfection
impersonated implant implying imposition imprecating imprimis
improvising impv inanenesses inaugurate incapably incentivize
incineration incloses incomparableness inconsequential incorporate
incrementing incumbered indecorous indentation indicative indignities
indistinguishably indoors indulges ineducation inerrable
inexperienced infants infestations infirmnesses inflicting
infracostal ingathered ingressions inheritances iniquity
injuriousnesses innervated inoculates inquisitionist insectile
insiders insolate inspirers instatement instr insulates intactness
intellects intensifies intercalations intercontinental interferon
interlarded intermarrying internalizing interpersonally
interrelatednesses intersperse interviewees intolerance
intransigents introducing intubates invades inventing inveterate
invocate iodides irenicism ironsmith irreducibly irresistibility
irriguous isobarisms isometrically issuable itineracies jackdaws
jaggery jangling javelins jeeringly jeremiad jeweler jigsawing jitter
jocosity jokester jot jowls judicative juicy jungly jurists juxtaposed
kalpa karstify keddah kendo kermesses keynote kibbutznik kidnaper
kilogram kindred kingpins kissers klatch kneads knobbed knowingest
kookaburras kruller labefaction labyrinths lacquer laddered lagoons
lambency laminates lancinate landscapist lankiness lapse larked lasso
laterite laudableness laundrywomen lawgiver laypersons leafhoppers
leapfrogs leaven leeches legated legislature leitmotifs lenients
leprous letterheads levelling lexicographically liberalists
librettist licorice lifesaving lightheadedly likelier limekiln limped
lines linkers lipoma liquidator listeners litharge litmus
liverishnesses loamier lobeline locative locutionary loggier loiterer
longevity loomed loping lotion louts lowboys luaus lucrativeness lulus
lumpier lungi lush luthern lymphangial lythraceous machinists maculate
maggot magnetochemistry maharani maimers majored malaprops malignants
maloti mammary manchineel manfully manicotti manipulativenesses
mansards manufactories maraschino margin markdown marooning marshland
mascaraing massaging masticate matchmark matings mattes mausoleum
mayflies mealworm meataxe medevaced medievalist meetings megavitamin
melded melodramatic memorableness mendaciousnesses mensurable
mercenaries mere meronymous mesmerizes mestee metallurgical
metastasize meterages meticulosity mewed microbe microcrystalline
micromanager microsporophyll midiron miffed milder militiamen
millesimal milometer mincing mingily minims minstrelsy mires
misanthropic miscalculate miscomprehended misdefines misery mishears
misled mispickel misrepresent misspending mistranslate miswriting
mixologists mobilizers moderators modulate mojo mollies momentum monde
monied monocles monographs monophyletic monotonousness moocher
moorages morality morion mortally moseyed motherly motorboat mouldering
mousers moveables mucky mudslides mulatto multicellularity
multipartite multivalences mundanities murkiest mushed muskiness
mutability mutisms mycelia myosotis mythicist nacred namable napkin
narghile nastiness nattering nauseations nearliest necessitate
necrophobia neg negotiators neologizes nephrotomy netiquette
neurophysiology newbie newspaper niccolite nielsbohriums nightlong
nincompoops nitpicked nix noddling nomadize nonadhesive noncandidates
nonconducting nondigestible nones nongreasy nonjoinder nonoccurrence
nonporousness nonrestrictive nonstaining nonuniform nooses northwards
nostalgic notepaper nourishment noyades nuclides numberless numskulls
nutmegged nymphaea oatmeal obis objurgators oblivious obsequiousness
obsoletism obtruding occlusions ocher octettes odeums offcuts
officiation ogival oilstone olestras omikron oncogenesis onsetting
oomphs openly ophthalmoscope opposites optimum orangutans
orchestrations ordn organophosphates origin ornithosis orthognathous
oscillatory ossuaries ostracized ounce outbreaks outearning outgrows
outlived outpoints outrunning outspends outwearing overabound
overbalance overcautious overcrowds overdubbing overexpanding
overgraze overindustrialize overlearning overoptimism overproducing
overripe overshadowing overspreading overstuff overtones overwind ow
oxidizing pacer packs paganish painstakingly palate palette pally
palsying pandemic panhandled pantheism papaws papped parading
parallelize paranoia parasitically pardners parietal parodied pars
participator partridgeberry passerines password pastors
paterfamiliases patination patrolman paunch pawnshops peacekeeper
peatbog peculator pedestrianism peduncles pegboard pellucidnesses
pendency penitentiary penstock pentylenetetrazol peptidase perched
perennial performing perigynous peripheralize perjurer permissively
perpetuals persistency perspicuously perturbingly pesky petcock
petrologists pfennige pharmacies phenformin philanderers
philosophically phonecards phosgenes photocomposer photogenic photons
phototype phylloid physiotherapeutics picadores pickup pieces pigging
pilaster pillion pimples pinioned pinpricks pipers pirogi pit
pitifullest pizza placental plainly planing plasmin platforming
playacts playwrights plectra pleurisy plopped plug plumule plussed
poaches poetasters pointless polarize policyholder polkaed
polyadelphous polygraphing polyphonous pomace ponderers pooch poplar
porcelains portableness portly positioning postage posthumously
postponed potages potholed poulard powdering practised pranksters
preadapt preassigning precentors precipitous preconditions predefined
predictors preengage prefers prehumans premedical prenotification
preplanning prepuberty presbytery presentation presidia prestissimo
preterites prevailer prewarmed priding primitively principalships
prisage privileged probed prochurch proctoscope products proficients
prognathism prohibiting proletarianisms prominence promulgates
proofreading property proportions prorate proselytize prosthesis
proteins prototypic provenances provitamin prudish pseudonymities
psychoanalysts psychoneuroses psychrometer publishable pufferies
pullet pulses punchy punkins purchased purities pursers pushover
putridity pylons pyrogenous pzazz quadricepses quaff qualmish quarriers
quasilinear queerness questionnaires quieten quintals quislings quoits
rabidness racketeers radiative radioisotope radiotherapists ragingly
rainband rakishness rampagers rands raped rare raspy ratiocinator
rattlebrain ravening razz reactivation readoption realm reapportioning
reasoning reattempts rebidding rebuts recapitulatory receptiveness
recipes reckonings recognizee recommendatory reconciled reconnoiters
recontaminated recoupments recruits recumbently redact redefine
redheaded redistributable redraw redwing reeled reenlistment reexports
refiles reflate reflowing refortified refried refuses regelate
registrant regretting rehabilitative reigning reinduced reinstalled
reinvesting rejoining relations relegates religiosities reluctivity
remastered reminisce remodifying remounted rends renovate reordered
repartee repel rephrase replicate repossessing reprint reprogramed
repugnantly requiter rescheduling resegregate resettled residually
resold resourcefulness respondent restating restrainedly resubmission
resurveyed retaliating retiarius retorsion retreated retrofitting
returning revanchism reverberated reverted revitalization
revolutionize rewind rhapsodizing rhizogenic rhythms ricketinesses
ridicule righteous rilles rinks rippliest ritualize riyals roast rockery
roguish romanizations rookiest roquelaure rotation rotundity rounder
routinizing rubberize rubricated ruefully ruining rummaged runic
russets ruttish sackers sacrosanctly safeguarding said salaciousness
salinity salsas salutatorians sampan sandbag saned santonin
saprophagous sarnies satem saturant savaged sawbucks scablike scalp
scant scared scatter schedulers schizophrenics schnauzers schoolmarms
scintillae scleroses scoped scotched scram scratchiness screwball
scripting scrubwomen scrutinizing scumbled scuttled seals seasickness
seccos secretions secularizing seditiousnesses seeking segregators
seize selfish semeiology seminarian semitropical sensate sensors
sentimo septicemic sequentially serener serine serums
sesquicentennials seventeen sexiest sforzandos shadowing shallot
shampooing sharking shearer sheered shelters shifter shiner shipper
shitted shoaled shofroth shorebirds shortsightedly showboated shrank
shrines shucking shuttlecocks sickeningly sideling sidewise sigil
signifiers siliceous silty simony simulative singled sinkings sirrah
situps skateboarder sketchpad skim skirmished skulkers skywalk slander
slating sleaziest sleepyheads slicking slink slitting slot slub
slumlords smallest smattered smilier smokers smriti snailfish snatch
snides snitching snooze snowblowers snub soapboxing socialite sockeyes
softest sold solicitings solleret sombreros somnolencies sons sopor
sorites soubrette soupspoon southpaw spaces spandex sparkers spatially
speccing specking spectroscopists speedsters spermatics sphincter
spiffied spindlings spirals spitball splayfeet splitter spokeswomen
spooled sportily spousals sprightliness sprogs spurner squalene
squattered squelches squirms stablish staggerings stalactitic stamp
stands starflower starwort stations stayed steamroll steeplebush
stemmatics stepfathers stereos steroid sticks stillage stinker
stirringly stockpiling stomaching stopcock stormers strabismuses
strainer strappado strawberries streetwise striae strikeouts strives
stroppiest stubbed study stunting style suavity subchloride subdeb
subfields subjoin sublittoral subnotebooks subprograms subside
substantial subtenants subtreasuries succeeding sucked sufferers
sugarier sulfaguanidine sulphating summerhouse sunbonnets sunned
superagency supercontinent superheroes supernatural superscribing
superthin supplest suppositive surcease surfs surprise survey
suspiration svelte swamplands swashes sweatshop swellhead swindling
switching sworn syllabuses sympathetics synchrocyclotron syndic
synonymously syringed tablatures tabulation tackling taiga takas talker
tamarisks tangential tans taproom tarpapers taskmaster tattiest
tautologically taxied teacup tearjerkers technocracies teepee
telegenic telephony telexed temperaments temptress tenderizing tensed
tenuring tergal terned terror testatrices tetherball textile thatched
their theorem thereof thermometers thewy thimerosal thirsty
thoroughwort threateningly thrived through thumbnails thwacks
ticketing tie til timekeepers timorousness tinkers tippers tisane
titrating toastmaster toff toking tomb tongs toolmakings topes topple
torose tortilla totalizing touchlines tousling townsmen trachea
tradeable tragedienne traitorous trances transcendentalists
transferrable tranship translating transmogrifying transportable
transvestism traumatize treachery treed trenail tressing tribeswoman
trichromatism triennials trikes trims triplicate tristich trivializes
trombonist trots trouts trued trunnion tryster tubes tulle tundras turban
turgescence turnround tutelar tweedinesses twill twit tympanum typists
tzarists ulcered ultramodern umbles unaccountability unamended
unassertivenesses unbanned unblocked unbundled uncertified unclaimed
uncoated unconcerns unconvinced uncrossing undefined underbodice
underemphasize undergrowth underpayment undershirts understudy
underwritten undissolved unearthed unentered unexpended unfeeling
unforeseen unfussy unhair unhinges unifilar unimproved uninvitingly
universalization unknowns unlimbering unman unmet unnaturalness
unornament unperturbed unprecedentedly unproportionate unread
unreflecting unreproducible unripe unsatisfying unseaworthiness
unsharable unsociable unstacking unsubtly untactfully untied untruest
unveils unwilled unyokes upheave upraised upstart upwind urethrae
urtexts usurers uvula vacillators vailed validation valvule vanities
varia variously vassaled vav veggies velours venerator ventrals
verbalizes verification vernacularized verticality vestigially via
vicariously victoriousness viewpoint villainies vines violoncellist
virtual viscus vital vitrify viviparous vocalizers voidable volleys
volutes vouches vulcanology wackos waggery wainwrights waling wallowing
wanking wardroom warmup wartiest washwoman watchman watermarks waverer
wayzgoose weariest weatherstripped weediness weevil welcomed
wentletrap whackers wheatworm whelp whf whinged whirl whistles whithers
wholesomeness whosoever widows wikiup willowier windburned windsail
wingspread winterkilled wisecracking witchgrass witling wobbliest
womanliness woodcut woodworking woozy working worldwide worthiest
wrappings wretched writhe wynd xylophone yardarm yea yelped yippee yoni
yuks zealotry zigzagger zitherists zoologists zygosis');

SELECT docid FROM t1 WHERE content MATCH 'abbrev zygosis';

SELECT docid FROM t1 WHERE content MATCH 'abbrev NEAR zygosis';

SELECT docid FROM t1 WHERE content MATCH 'abbrev NEAR/100 zygosis';

SELECT docid FROM t1 WHERE content MATCH 'abbrev NEAR/1000 zygosis';

SELECT docid FROM t1 WHERE content MATCH 'abbrev NEAR/10000 zygosis';

SELECT docid FROM t1 WHERE content MATCH 'three NEAR/1 one';

SELECT docid FROM t1 WHERE content MATCH '"one two" NEAR/1 five';

SELECT docid FROM t1 WHERE content MATCH '"one two" NEAR/2 five';

SELECT docid FROM t1 WHERE content MATCH 'one NEAR four';

SELECT docid FROM t1 WHERE content MATCH 'four NEAR three';

-- ===fts3query.test===
CREATE VIRTUAL TABLE t1 USING fts3(x);
BEGIN;
INSERT INTO t1 VALUES('The source code for SQLite is in the public');

COMMIT;

CREATE VIRTUAL TABLE zoink USING fts3;
INSERT INTO zoink VALUES('The apple falls far from the tree');

SELECT docid FROM zoink WHERE zoink MATCH '(apple oranges) AND apple';

SELECT docid FROM zoink WHERE zoink MATCH 'apple AND (oranges apple)';

CREATE VIRTUAL TABLE foobar using FTS3(description, tokenize porter);
INSERT INTO foobar (description) values ('
Filed under: Emerging Technologies, EV/Plug-in, Hybrid, Chevrolet, GM, 
ZENN 2011 Chevy Volt - Click above for high-res image gallery There are 
16 days left in the month of December. Besides being time for most 
Americans to kick their Christmas shopping sessions into high gear and
start planning their resolutions for 2010, it also means that there''s
precious little time for EEStor to "deliver functional technology" to
Zenn Motors as promised. Still, the promises held out by the secretive
company are too great for us to forget about entirely. We''d love for
EEStor''s claims to be independently verified and proven accurate, as
would just about anyone else looking to break free of petroleum in fav
');

SELECT docid FROM foobar WHERE description MATCH '"high sp d"';

SELECT mit(matchinfo(foobar)) FROM foobar WHERE foobar MATCH 'the';

DROP TABLE IF EXISTS t1;
CREATE TABLE t1(number INTEGER PRIMARY KEY, date);
CREATE INDEX i1 ON t1(date);
CREATE VIRTUAL TABLE ft USING fts3(title);
CREATE TABLE bt(title);

-- ===fts3rnd.test===
INSERT INTO t1(docid, a, b, c) VALUES(rowid, a, b, c);

DELETE FROM t1 WHERE rowid = rowid;

-- ===fts3snippet.test===
CREATE VIRTUAL TABLE ft USING fts3;
INSERT INTO ft VALUES('xxx xxx xxx xxx');

INSERT INTO ft VALUES('one' || commas || 'two');

DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3;
INSERT INTO ft VALUES(ten);
INSERT INTO ft VALUES(ten || ' ' || ten);

DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3(x, y);

INSERT INTO ft(docid, x, y) VALUES(docid, v1, v2);

DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3(a, b);
INSERT INTO ft VALUES(v1, numbers);
INSERT INTO ft VALUES(v1, NULL);

UPDATE ft_content SET c1b = 'hello world' WHERE c1b = numbers;

DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3;
INSERT INTO ft VALUES('one two three four five six seven eight nine ten');

INSERT INTO ft VALUES(
'one two three four five '
|| 'six seven eight nine ten '
|| 'eleven twelve thirteen fourteen fifteen '
|| 'sixteen seventeen eighteen nineteen twenty '
|| 'one two three four five '
|| 'six seven eight nine ten '
|| 'eleven twelve thirteen fourteen fifteen '
|| 'sixteen seventeen eighteen nineteen twenty'
);

DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3(a, b, c);
INSERT INTO ft VALUES(
'one two three four five', 
'four five six seven eight', 
'seven eight nine ten eleven'
);

UPDATE ft SET b = NULL;

DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3(x);
INSERT INTO ft VALUES(numbers);

BEGIN;
DROP TABLE IF EXISTS ft;
CREATE VIRTUAL TABLE ft USING fts3(x);