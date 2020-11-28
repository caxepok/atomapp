import React from 'react'
import classes from 'classnames'
import './layout.scss'

const LayoutSection = function (props) {
  const { header, children, aside, menu } = props
  return <div 
    className={ classes('layout-section', aside && '-aside', menu && '-menu')}
  >
    {header && <div className='layout-subheader'>
      { header }
    </div>}
    <div className='layout-content'>
      { children }
    </div>
  </div>
}

export default LayoutSection