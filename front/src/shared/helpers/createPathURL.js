export const createPathURL = function (path, params) {
  if (!params || !path) {
    return ''
  }

  let url = path

  Object.keys(params).forEach(function(key) {
    if (key.toString() !== '0') {
      const replacement = params[key] ? `${params[key]}` : ''
      url = url
        .replace(`:${key}?`, `:${key}`)
        .replace(`/:${key}`, replacement ? `/${replacement}` : '')
        .replace(`:${key}`, replacement)
    } else {
      url = url.replace('*', params[key])
    }
  })

  return url.substr(-1) === '/'
    ? url.substr(0, url.length - 1)
    : url
}