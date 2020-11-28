import React from 'react'
import classes from 'classnames'
import { PRIORITIES } from '../../shared/consts'
import './datum.scss'

const DatumPriority = function (props) {
  const { priority } = props 
  
  return <span 
    className={classes('datum-priority', `-type${priority}`)} 
    title={PRIORITIES[+priority] && `Приоритет: ${PRIORITIES[+priority]}`}
  />
}

export default DatumPriority