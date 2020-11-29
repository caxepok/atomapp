import React from 'react'
import LayoutSection from '../../components/Layout/LayoutSection'
import './summary.scss'

const Summary = function () {
  return <LayoutSection>
    <div className='summary-images'>
      <img src='/g1.png' alt='' />
      <img src='/g2.png' alt='' />
    </div>
  </LayoutSection>
}

export default Summary