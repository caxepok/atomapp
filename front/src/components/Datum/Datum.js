import React from 'react'
import classes from 'classnames'
import './datum.scss'
import { Loader } from '../../shared'

const Datum = function (props) {
  const { label, children, large } = props 
  
  return <dl className={classes('datum', large && '-large')}>
    <dt>{ label === undefined ? <Loader /> : label }</dt>
    { children }
  </dl>
}

export default Datum
  
