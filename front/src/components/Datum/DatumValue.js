import React from 'react'
import { Loader } from '../../shared'
import { getFormattedDate } from '../../shared/helpers'
import { shallowEqual, useSelector } from 'react-redux'

const DatumValue = function (props) {
  const { value, type } = props 
  const workers = useSelector(({ data }) => data.workers, shallowEqual)
  
  const getValue = function () {
    switch (type) {
      case 'date':
        return getFormattedDate(value)
      case 'person':
        return workers 
          ? workers.find(x => +x.id === +value).name
          : <Loader />
      default:
        return value
    }
  }
  
  if (value === undefined) {
    return <dd><Loader /></dd>
  }
  
  if (value === null) {
    return <dd>—</dd>
  }
  
  return <dd>
    { getValue() }
  </dd>
}

export default DatumValue