import React from 'react'
import classes from 'classnames'
import './datum.scss'

const DatumStatus = function (props) {
  const { className, children } = props 
  
  return <span className={classes('datum-status', className)}>
    { children }
  </span>
}

export default DatumStatus