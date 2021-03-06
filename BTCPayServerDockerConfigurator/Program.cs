using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTCPayServerDockerConfigurator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddEnvironmentVariables(prefix: "CONFIGURATOR_");
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
-----BEGIN CERTIFICATE-----

MIIB8TCCAVoCCQCg2ZYlANUEvjANBgkqhkiG9w0BAQsFADA9MQswCQYDVQQGEwJV

UzELMAkGA1UECAwCQ0ExITAfBgNVBAoMGEludGVybmV0IFdpZGdpdHMgUHR5IEx0

ZDAeFw0xNDA4MTgyMzE5NDJaFw0xNTA4MTgyMzE5NDJaMD0xCzAJBgNVBAYTAlVT

MQswCQYDVQQIDAJDQTEhMB8GA1UECgwYSW50ZXJuZXQgV2lkZ2l0cyBQdHkgTHRk

MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDV4suKtPRyipQJg35O/wIndwm+

5RV+s+jqo8VS7tJ1E4OIsSMo7eVuNU4pLTIqehNN+Skyk/i17y6cPwo2Mff+E6VB

lJrjNLO+rI+B7Ttx7Cs9imoE38Pmv0LKzQbAz8Uz3T6zxXHJpjIWA4PKiw+mO6qw

niEDDutypPa2mB+KjQIDAQABMA0GCSqGSIb3DQEBCwUAA4GBAHUfkcY4wNZZGT3f

oCoB0cNy+gtS86Iu2XU+WzKWxQxvgSiloQ2l0NDsRlw9wBQQZNQOJtPNfTIXkpfU

NoD7qU0Dd0TawoIRAetWzweW0PIJt+Dh7/z7FUTXg5p2IRhOPVNA9+K1wBGfOkEF

6cYkdpr0FmQ52L+Vc1QcNCxwYtWm

-----END CERTIFICATE-----

resource "google_compute_network" "mesos-global-net" {

    name                    = "${var.name}-global-net"

    auto_create_subnetworks = false # custom subnetted network will be created that can support google_compute_subnetwork resources

}



resource "google_compute_subnetwork" "mesos-net" {

    name          = "${var.name}-${var.region}-net"

    ip_cidr_range = "${var.subnetwork}"

    network       = "${google_compute_network.mesos-global-net.self_link}" # parent network

}
var Buffer = require('safe-buffer').Buffer



module.exports = function base (ALPHABET) {

  var ALPHABET_MAP = {}

  var BASE = ALPHABET.length

  var LEADER = ALPHABET.charAt(0)



  // pre-compute lookup table

  for (var z = 0; z < ALPHABET.length; z++) {

    var x = ALPHABET.charAt(z)



    if (ALPHABET_MAP[x] !== undefined) throw new TypeError(x + ' is ambiguous')

    ALPHABET_MAP[x] = z

  }



  function encode (source) {

    if (source.length === 0) return ''



    var digits = [0]

    for (var i = 0; i < source.length; ++i) {

      for (var j = 0, carry = source[i]; j < digits.length; ++j) {

        carry += digits[j] << 8

        digits[j] = carry % BASE

        carry = (carry / BASE) | 0

      }



      while (carry > 0) {

        digits.push(carry % BASE)

        carry = (carry / BASE) | 0

      }

    }



    var string = ''



    // deal with leading zeros

    for (var k = 0; source[k] === 0 && k < source.length - 1; ++k) string += ALPHABET[0]

    // convert digits to a string

    for (var q = digits.length - 1; q >= 0; --q) string += ALPHABET[digits[q]]



    return string

  }



  function decodeUnsafe (string) {

    if (string.length === 0) return Buffer.allocUnsafe(0)



    var bytes = [0]

    for (var i = 0; i < string.length; i++) {

      var value = ALPHABET_MAP[string[i]]

      if (value === undefined) return



      for (var j = 0, carry = value; j < bytes.length; ++j) {

        carry += bytes[j] * BASE

        bytes[j] = carry & 0xff

        carry >>= 8

      }



      while (carry > 0) {

        bytes.push(carry & 0xff)

        carry >>= 8

      }

    }



    // deal with leading zeros

    for (var k = 0; string[k] === LEADER && k < string.length - 1; ++k) {

      bytes.push(0)

    }



    return Buffer.from(bytes.reverse())

  }



  function decode (string) {

    var buffer = decodeUnsafe(string)

    if (buffer) return buffer



    throw new Error('Non-base' + BASE + ' character')

  }



  return {

    encode: encode,

    decodeUnsafe: decodeUnsafe,

    decode: decode

  }

}



},{"safe-buffer":65}],4:[function(require,module,exports){

'use strict'



exports.byteLength = byteLength

exports.toByteArray = toByteArray

exports.fromByteArray = fromByteArray



var lookup = []

var revLookup = []

var Arr = typeof Uint8Array !== 'undefined' ? Uint8Array : Array



var code = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/'

for (var i = 0, len = code.length; i < len; ++i) {

  lookup[i] = code[i]

  revLookup[code.charCodeAt(i)] = i

}



revLookup['-'.charCodeAt(0)] = 62

revLookup['_'.charCodeAt(0)] = 63



function placeHoldersCount (b64) {

  var len = b64.length

  if (len % 4 > 0) {

    throw new Error('Invalid string. Length must be a multiple of 4')

  }



  // the number of equal signs (place holders)

  // if there are two placeholders, than the two characters before it

  // represent one byte

  // if there is only one, then the three characters before it represent 2 bytes

  // this is just a cheap hack to not do indexOf twice

  return b64[len - 2] === '=' ? 2 : b64[len - 1] === '=' ? 1 : 0

}



function byteLength (b64) {

  // base64 is 4/3 + up to two characters of the original data

  return b64.length * 3 / 4 - placeHoldersCount(b64)

}



function toByteArray (b64) {

  var i, j, l, tmp, placeHolders, arr

  var len = b64.length

  placeHolders = placeHoldersCount(b64)



  arr = new Arr(len * 3 / 4 - placeHolders)



  // if there are placeholders, only get up to the last complete 4 chars

  l = placeHolders > 0 ? len - 4 : len



  var L = 0



  for (i = 0, j = 0; i < l; i += 4, j += 3) {

    tmp = (revLookup[b64.charCodeAt(i)] << 18) | (revLookup[b64.charCodeAt(i + 1)] << 12) | (revLookup[b64.charCodeAt(i + 2)] << 6) | revLookup[b64.charCodeAt(i + 3)]

    arr[L++] = (tmp >> 16) & 0xFF

    arr[L++] = (tmp >> 8) & 0xFF

    arr[L++] = tmp & 0xFF

  }



  if (placeHolders === 2) {

    tmp = (revLookup[b64.charCodeAt(i)] << 2) | (revLookup[b64.charCodeAt(i + 1)] >> 4)

    arr[L++] = tmp & 0xFF

  } else if (placeHolders === 1) {

    tmp = (revLookup[b64.charCodeAt(i)] << 10) | (revLookup[b64.charCodeAt(i + 1)] << 4) | (revLookup[b64.charCodeAt(i + 2)] >> 2)

    arr[L++] = (tmp >> 8) & 0xFF

    arr[L++] = tmp & 0xFF

  }



  return arr

}



function tripletToBase64 (num) {

  return lookup[num >> 18 & 0x3F] + lookup[num >> 12 & 0x3F] + lookup[num >> 6 & 0x3F] + lookup[num & 0x3F]

}



function encodeChunk (uint8, start, end) {

  var tmp

  var output = []

  for (var i = start; i < end; i += 3) {

    tmp = (uint8[i] << 16) + (uint8[i + 1] << 8) + (uint8[i + 2])

    output.push(tripletToBase64(tmp))

  }

  return output.join('')

}



function fromByteArray (uint8) {

  var tmp

  var len = uint8.length

  var extraBytes = len % 3 // if we have 1 byte left, pad 2 bytes

  var output = ''

  var parts = []

  var maxChunkLength = 16383 // must be multiple of 3



  // go through the array every three bytes, we'll deal with trailing stuff later

  for (var i = 0, len2 = len - extraBytes; i < len2; i += maxChunkLength) {

    parts.push(encodeChunk(uint8, i, (i + maxChunkLength) > len2 ? len2 : (i + maxChunkLength)))

  }



  // pad the end with zeros, but make sure to not forget the extra bytes

  if (extraBytes === 1) {

    tmp = uint8[len - 1]

    output += lookup[tmp >> 2]

    output += lookup[(tmp << 4) & 0x3F]

    output += '=='

  } else if (extraBytes === 2) {

    tmp = (uint8[len - 2] << 8) + (uint8[len - 1])

    output += lookup[tmp >> 10]

    output += lookup[(tmp >> 4) & 0x3F]

    output += lookup[(tmp << 2) & 0x3F]

    output += '='

  }



  parts.push(output)



  return parts.join('')

}



},{}],5:[function(require,module,exports){

// (public) Constructor

function BigInteger(a, b, c) {

  if (!(this instanceof BigInteger))

    return new BigInteger(a, b, c)



  if (a != null) {

    if ("number" == typeof a) this.fromNumber(a, b, c)

    else if (b == null && "string" != typeof a) this.fromString(a, 256)

    else this.fromString(a, b)

  }

}



var proto = BigInteger.prototype



// duck-typed isBigInteger

proto.__bigi = require('../package.json').version

BigInteger.isBigInteger = function (obj, check_ver) {

  return obj && obj.__bigi && (!check_ver || obj.__bigi === proto.__bigi)

}



// Bits per digit

var dbits



// am: Compute w_j += (x*this_i), propagate carries,

// c is initial carry, returns final carry.

// c < 3*dvalue, x < 2*dvalue, this_i < dvalue

// We need to select the fastest one that works in this environment.



// am1: use a single mult and divide to get the high bits,

// max digit bits should be 26 because

// max internal value = 2*dvalue^2-2*dvalue (< 2^53)

function am1(i, x, w, j, c, n) {

  while (--n >= 0) {

    var v = x * this[i++] + w[j] + c

    c = Math.floor(v / 0x4000000)

    w[j++] = v & 0x3ffffff

  }

  return c

}
}
#' Get the logged in user's email and other info
#' 
#' @param id ID of the person to get the profile data for. 'me' to get current user.
#' 
#' @return A People resource
#' 
#' https://developers.google.com/+/web/api/rest/latest/people#resource-representations
#' 
#' @seealso https://developers.google.com/+/web/api/rest/latest/people
#' 
#' @export
#' 
#' @examples 
#' 
#' \dontrun{
#' library(googleAuthR)
#' library(googleID)
#' options(googleAuthR.scopes.selected = 
#'    c("https://www.googleapis.com/auth/userinfo.email",
#'      "https://www.googleapis.com/auth/userinfo.profile"))
#'                                         
#' googleAuthR::gar_auth()
#' 
#' ## default is user logged in
#' user <- get_user_info()
#' }
#' 
get_user_info <- function(id = "me"){
  
  url <- sprintf("https://www.googleapis.com/plus/v1/people/%s", id)
  
  g <- googleAuthR::gar_api_generator(url, "GET")
  
  req <- g()
  
  req$content
  
}
#' Whitelist check
#' 
#' After a user logs in, check to see if they are on a whitelist
#' 
#' @param user_info the object returned by \link{get_user_info}
#' @param whitelist A character vector of emails on whitelist
#' 
#' @return TRUE if on whitelist or no whitelist, FALSE if not
#' @export
#' 
#' @examples 
#' 
#' \dontrun{
#' library(googleAuthR)
#' library(googleID)
#' options(googleAuthR.scopes.selected = 
#'    c("https://www.googleapis.com/auth/userinfo.email",
#'      "https://www.googleapis.com/auth/userinfo.profile"))
#'                                         
#' googleAuthR::gar_auth()
#' 
#' ## default is user logged in
#' user <- get_user_info()
#' 
#' the_list <- whitelist(user, c("your@email.com", 
#'                               "another@email.com", 
#'                               "yet@anotheremail.com"))
#' 
#' if(the_list){
#'   message("You are on the list.")
#' } else {
#'   message("If you're not on the list, you're not getting in.")
#'}
#' 
#' 
#' 
#' }
whitelist <- function(user_info, whitelist = NULL){
  
  if(user_info$kind != "plus#person"){
    stop("Invalid user object used for user_info")
  }
  
  out <- FALSE
  
  if(is.null(whitelist)){
    message("No whitelist found")
    out <- TRUE
  }
  
  check <- user_info$emails$value
  
  if(is.null(check)){
    stop("No user email found")
  }
  
  if(any(check %in% whitelist)){
    message(check, " is in whitelist ")
    out <- TRUE
  } else {
    message(check, " is NOT on whitelist")
  }
  
  out
  
}
