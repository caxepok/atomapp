import React from 'react'
import classes from 'classnames'
import './control.scss'

const Control = function (props) {
  const { label, children, inline } = props
  
  return <div className={classes('control', inline && '-inline')}>
    {label && <span className='control-label'>
      {label}
    </span> }
    <span className='control-children'>
      { children }
    </span>
  </div>
}

export default Control