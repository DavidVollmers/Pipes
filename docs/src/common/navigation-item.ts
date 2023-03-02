import type { ForwardRefExoticComponent, SVGProps } from 'react'

export interface NavigationItem {
  readonly name: string
  readonly icon: ForwardRefExoticComponent<SVGProps<SVGSVGElement>>
  readonly isActive: boolean
  readonly href: string
}
