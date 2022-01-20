<div>
  If you want to use the application to maintain your investments then use the following URL, where I have hosted the application
  https://portfoliomanager.ddns.net
  </div>
<div>
  How to build & run the application.(I have also given below description for each project)
  <ul>
    <li>For Web Application - Get everything from 'Analytics' repository</li>
    <li>For dependency on database - Get everything from 'DataAccessLayer' repository</li>
    <li>In DataAccesslayer, under SQL_statement, use file dbschema.pdf </li>
    <li>This file has the dbschema using which you will need to create SQLite db</li>
    <li>Name the dbfile as mfdata.db and store it under root\ProdDatabase</li>
    <li>Open Analytics solution in visual studio and add DataAccessLayer as dependency</li>
    <li>You should be able to build the solution</li>
    <li>Extract repository Pull Data and build the solution</li>
    <li> Pull data allows you to update master data. Such as mutual fund list, stocks from NSE</li>
  </ul>
</div>
<div>Analytics project
  <ul>
    <li>ASP.Net based front end for managing stocks, mutual fund, etf or any investment that is associated with any of the global exchanges</li>
    <li>The application stores any new symbols that you search in the application and which  are not available currently in the application symbol repository</li> 
    <li>You can use the applicatin to manage multiple portfolios</li>
    <li>All mutual funds available in India that are coming under SEBI are managed separatly under 'Mutual Funds'</li>
    <li> Any other investment type that you search, which is listed on any global exchange is managed by application separatly under 'Stocks'</li>
    <li>For example if you search a ETF fund with its symbol and if it is not currently available in application repository then it gets added under 'Stocks'</li>
    <li>You do not need to create your portfolio but can use the application for your investment research and decision making</li>
  </ul>
  </div>
