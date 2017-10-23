# linqpad-soap-driver

> This project is based on the [`linqpad-soap-driver` from dylanmei](https://github.com/dylanmei/linqpad-soap-driver). As the origin repository seems not to be maintained anymore I've implemented few more features (see list below).

A SOAP service driver for LINQPad. Useful for invoking SOAP-based services with minimal pain and fuss. For the best experience, this goes especially well with the auto-complete support in [LINQPad Pro](http://www.linqpad.net/Purchase.aspx).

[![Build status](https://ci.appveyor.com/api/projects/status/b3qel2k78pok0l50/branch/master?svg=true)](https://ci.appveyor.com/project/flex87/linqpad-soap-driver/branch/master)

## setup

- Download the [latest *.lpx](https://github.com/flex87/linqpad-soap-driver/releases/latest) (depending on your LINQPad Version) from the [release page](https://github.com/flex87/linqpad-soap-driver/releases). 
- In LINQPad load the driver by choosing:

   *Add Connection*  
   *View more drivers*  
   *Browse to a .LPX file*

- Complete the connection dialog by entering the HTTP URL of your service and then selecting the SOAP binding you wish to use.
- Reveal the endpoint operations by expanding your new connection in the LINQPad explorer view.

## troubleshooting
* When connecting, the driver is looking for the service's WSDL file by looking in common locations. If you're having trouble here, try specifying the exact HTTP URL of the WSDL file as you'd see it in your browser.
* .NET DateTime fields don't work very well.
* Nullable fields don't work very well. For such fields, an extra *Specified* field is generated. You must set this extra field as *true* if you are specifying a value for your Nullable field.

## contributing

1. Fork it
2. Create your feature branch `git checkout -b my-new-feature`
3. Commit your changes `git commit -am 'Added some feature'`
4. Push to the branch `git push origin my-new-feature`
5. Create new Pull Request
