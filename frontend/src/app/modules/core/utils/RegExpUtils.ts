﻿export class RegExpUtils {
  public static GetEmailRegExp(): RegExp {
    return /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
  }

  public static GetPasswordRegExp(): RegExp {
    return /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
  }
}
