import React from 'react'
import { Loader } from '../index'
import { format, utcToZonedTime } from 'date-fns-tz'
const timeZone = 'Europe/Moscow'

export const getFormattedDate = function (date) {
  if (date === false) {
    return <Loader />
  }
  
  if (!date) {
    return '—'
  }
  
  return format(
    utcToZonedTime(new Date(date), timeZone),
    'dd.MM.yyyy HH:mm',
    { timeZone }
  )
}

export const getFormattedPeriod = function (from, to) {
  if (!from && !to) {
    return null
  }

  const timeZone = 'Europe/Moscow'

  const startDate = from ? format(
    utcToZonedTime(new Date(from), timeZone),
    'dd.MM.yyyy HH:mm',
    { timeZone }
  ) : ''

  const endDate = to ? format(
    utcToZonedTime(new Date(to), timeZone),
    'dd.MM.yyyy HH:mm',
    { timeZone }
  ) : ''

  return [startDate, endDate].map(x => x || '...').join(' — ')
}
